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

    ''' <summary> Contracts this class setting the meta status to
    ''' <see cref="MetaStatusBits">None</see>. </summary>
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
        Me._Value = MetaStatusBits.None
    End Sub

    ''' <summary> Presets the meta status. </summary>
    Public Sub Preset(ByVal value As Long)
        Me._Value = CType(value, MetaStatusBits)
    End Sub

#End Region

#Region " PROPERTIES "

    ''' <summary> Returns a short description representing the measurand meta status. </summary>
    ''' <returns> The short meta status description. </returns>
    Public Function ToShortDescription() As String
        If Not Me.IsSet Then
            ' if not set then return NO
            Return "no"
        ElseIf Not Me.HasValue Then
            ' if no value we are out of luck
            Return "nv"
        ElseIf Me.IsValid Then
            ' if valid, it can be high, low, or passed
            If Me.IsPass Then
                Return "p"
            ElseIf Me.IsHigh Then
                Return "hi"
            ElseIf Me.IsLow Then
                Return "lo"
            End If
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining valid short outcome.")
            Return "na"
        Else
            ' if not valid then it can be contact check or compliance. 
            If Me.FailedContactCheck Then
                Return "cc"
            ElseIf Me.HitCompliance Then
                Return "c"
            ElseIf Me.HitRangeCompliance Then
                Return "rc"
            End If
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining invalid short outcome.")
            Return "na"
        End If
    End Function

    ''' <summary> Returns a long description representing the measurand meta status. </summary>
    ''' <returns> The long  meta status description. </returns>
    Public Function ToLongDescription() As String
        If Not Me.IsSet Then
            ' if not set then return NO
            Return "Not Set"
        ElseIf Not Me.HasValue Then
            ' if no value we are out of luck
            Return "No Value"
        ElseIf Me.IsValid Then
            ' if valid, it can be high, low, or passed
            If Me.IsPass Then
                Return "Pass"
            ElseIf Me.IsHigh Then
                Return "High"
            ElseIf Me.IsLow Then
                Return "Low"
            End If
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining valid short outcome.")
            Return "NA"
        Else
            ' if not valid then it can be contact check or compliance. 
            If Me.FailedContactCheck Then
                Return "Contact Check"
            ElseIf Me.HitCompliance Then
                Return "Compliance"
            ElseIf Me.HitRangeCompliance Then
                Return "Range Compliance"
            End If
            ' if we do not return a value, raise an exception
            Debug.Assert(Not Debugger.IsAttached, "Unhandled case in determining invalid short outcome.")
            Return "NA"
        End If
    End Function

#End Region

#Region " CONDITIONS "

    ''' <summary> Gets or sets the condition determining if the outcome indicates a failed contact
    ''' check. </summary>
    ''' <value> The failed contact check. </value>
    Public Property FailedContactCheck() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.FailedContactCheck)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.FailedContactCheck, value)
            Me.Toggle(MetaStatusBits.Valid, Not value)
        End Set
    End Property

    ''' <summary> Returns true if the <see cref="MetaStatus"/> was set. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatus"/> was set; Otherwise, <c>False</c>. </returns>
    Public Function IsSet() As Boolean
        Return Me.Value - Measurand.MetaStatusBitsBase >= 0
    End Function

    ''' <summary> Gets or sets the condition determining if the outcome indicates a having a value,
    ''' namely if a value was set. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBits.HasValue"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HasValue() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.HasValue)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.HasValue, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a high value. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBits.High"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property IsHigh() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.High)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.High, value)
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
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBits.HitLevelCompliance"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HitLevelCompliance() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.HitLevelCompliance)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.HitLevelCompliance, value)
            Me.Toggle(MetaStatusBits.Valid, Not value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that
    ''' hit Status compliance. Status compliance is reported by the instrument in the status word. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBits.HitStatusCompliance"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HitStatusCompliance() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.HitStatusCompliance)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.HitStatusCompliance, value)
            Me.Toggle(MetaStatusBits.Valid, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that
    ''' Hit Range Compliance. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBits.HitRangeCompliance"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property HitRangeCompliance() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.HitRangeCompliance)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.HitRangeCompliance, value)
            Me.Toggle(MetaStatusBits.Valid, Not value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' low. </summary>
    ''' <returns> <c>True</c> if the <see cref="MetaStatusBits.Low"/> was set; Otherwise, <c>False</c>. </returns>
    Public Property IsLow() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.Low)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.Low, value)
        End Set
    End Property

    ''' <summary> Is affirmative. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>True</c> if the <paramref name="value"/> is set in the
    ''' <see cref="MetaStatus">meta status</see>; Otherwise, <c>False</c>. </returns>
    Public Function IsAffirmative(ByVal value As MetaStatusBits) As Boolean
        Return (value > 0) AndAlso (Me.Value > 0) AndAlso ((Me.Value And value) = value)
    End Function

    ''' <summary> Toggles the bits specified in the <paramref name="value"/> based on the
    ''' <paramref name="switch"/>. </summary>
    ''' <param name="value">  The value. </param>
    ''' <param name="switch"> True to set; otherwise, clear. </param>
    Public Sub Toggle(ByVal value As MetaStatusBits, ByVal switch As Boolean)
        If switch Then
            Me._Value = Me._Value Or value
        Else
            Me._Value = Me._Value And (Not value)
        End If
        If Not (value = MetaStatusBits.Pass OrElse value = MetaStatusBits.HasValue) Then
            Me.Toggle(MetaStatusBits.Pass, Me.Passed)
            Me.Toggle(MetaStatusBits.HasValue, True)
        End If
    End Sub

    ''' <summary> Gets or sets the condition determining if the outcome is Pass. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBits.Pass"/> was set; Otherwise, <c>False</c>. </value>
    Public Property IsPass() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.Pass)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.Pass, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome is Valid. The measured value is
    ''' valid when its value or pass/fail outcomes or compliance or contact check conditions are set. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBits.Valid"/> was set; Otherwise, <c>False</c>. </value>
    Public Property IsValid() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.Valid)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.Valid, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' infinity. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBits.Infinity"/> was set; Otherwise, <c>False</c>. </value>
    Public Property Infinity() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.Infinity)
        End Get
        Set(ByVal value As Boolean)
            Toggle(MetaStatusBits.Infinity, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' not a number. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBits.NotANumber"/> was set; Otherwise, <c>False</c>. </value>
    Public Property NotANumber() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.NotANumber)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.NotANumber, value)
        End Set
    End Property

    ''' <summary> Gets or sets the condition determining if the outcome indicates a measurement that is
    ''' negative infinity. </summary>
    ''' <value> <c>True</c> if the <see cref="MetaStatusBits.NegativeInfinity"/> was set; Otherwise, <c>False</c>. </value>
    Public Property NegativeInfinity() As Boolean
        Get
            Return Me.IsAffirmative(MetaStatusBits.NegativeInfinity)
        End Get
        Set(ByVal value As Boolean)
            Me.Toggle(MetaStatusBits.NegativeInfinity, value)
        End Set
    End Property

    ''' <summary> Returns True if passed. </summary>
    ''' <retuns> <c>True</c> if the status <see cref="IsValid"/> and 
    '''          not <see cref="FailedContactCheck"/> and
    '''          not <see cref="HitCompliance"/> and
    '''          not <see cref="HitRangeCompliance"/> and
    '''          not <see cref="IsLow"/> and
    '''          not <see cref="IsHigh"/> and
    '''          </retuns>
    Public Function Passed() As Boolean

        Return Me.IsValid AndAlso
            (Not (Me.FailedContactCheck OrElse Me.HitCompliance OrElse Me.HitRangeCompliance)) AndAlso
            (Not (Me.IsHigh OrElse Me.IsLow))

    End Function

    ''' <summary> Returns true if the value is valid, not out of range and not failed. </summary>
    ''' <returns> <c>True</c> if the value is valid, not out of range and not failed; Otherwise, <c>False</c> </returns>
    Public Function Valid() As Boolean
        Return Me.IsAffirmative(MetaStatusBits.Valid) AndAlso Not (
               Me.IsAffirmative(MetaStatusBits.NotANumber) OrElse
               Me.IsAffirmative(MetaStatusBits.Infinity) OrElse
               Me.IsAffirmative(MetaStatusBits.NegativeInfinity) OrElse
               Me.IsAffirmative(MetaStatusBits.FailedContactCheck) OrElse
               Me.IsAffirmative(MetaStatusBits.HitLevelCompliance) OrElse
               Me.IsAffirmative(MetaStatusBits.HitRangeCompliance))
    End Function

    Private _Value As MetaStatusBits

    ''' <summary> Holds the meta status value. </summary>
    ''' <value> The meta status value. </value>
    Public ReadOnly Property Value() As MetaStatusBits
        Get
            Return Me._Value
        End Get
    End Property

#End Region

End Class

