Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary>  Defines the contract that must be implemented by a Contact Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class ContactSubsystemBase
    Inherits SourceMeasureUnitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
#If False Then
        Me.NewAmount()
        Me._FunctionModeRanges = New SourceFunctionRangeDictionary
        For Each fm As SourceFunctionMode In [Enum].GetValues(GetType(SourceFunctionMode))
            Me._FunctionModeRanges.Add(fm, New Core.Pith.RangeR(0, 1))
        Next
#End If
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.ContactCheckOkay = New Boolean?
        Me.ContactCheckThreshold = New Integer?
        Me.ContactCheckSpeedMode = New Tsp.ContactCheckSpeedMode?
        Me.ContactResistances = ""
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

#Region " CONTACT CHECK "

#Region " CONTACT CHECK SPEED MODE "

    ''' <summary> The Contact Check Speed Mode. </summary>
    Private _ContactCheckSpeedMode As ContactCheckSpeedMode?

    ''' <summary> Gets or sets the cached Contact Check Speed Mode. </summary>
    ''' <value> The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property ContactCheckSpeedMode As ContactCheckSpeedMode?
        Get
            Return Me._ContactCheckSpeedMode
        End Get
        Protected Set(ByVal value As ContactCheckSpeedMode?)
            If Not Nullable.Equals(Me.ContactCheckSpeedMode, value) Then
                Me._ContactCheckSpeedMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Contact Check Speed Mode. </summary>
    ''' <param name="value"> The  Contact Check Speed Mode. </param>
    ''' <returns> The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if unknown. </returns>
    Public Function ApplyContactCheckSpeedMode(ByVal value As ContactCheckSpeedMode) As ContactCheckSpeedMode?
        Me.WriteContactCheckSpeedMode(value)
        Return Me.QueryContactCheckSpeedMode()
    End Function

    ''' <summary> Queries the Contact Check Speed Mode. </summary>
    ''' <returns> The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if unknown. </returns>
    Public Function QueryContactCheckSpeedMode() As ContactCheckSpeedMode?
        Dim currentValue As String = Me.ContactCheckSpeedMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(currentValue)
        currentValue = Me.Session.QueryPrintTrimEnd("{0}.contact.speed()", Me.SourceMeasureUnitReference)
        If String.IsNullOrWhiteSpace(currentValue) Then
            Dim message As String = "Failed fetching Contact Check Speed Mode"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.ContactCheckSpeedMode = New Tsp.ContactCheckSpeedMode?
        Else
            Dim se As New StringEnumerator(Of ContactCheckSpeedMode)
            ' strip the SMU reference.
            currentValue = currentValue.Substring(currentValue.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1).Trim("."c)
            Me.ContactCheckSpeedMode = se.ParseContained(currentValue.Substring(4))
        End If
        Return Me.ContactCheckSpeedMode
    End Function

    ''' <summary> Writes the Contact Check Speed Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Contact Check Speed Mode. </param>
    ''' <returns> The <see cref="ContactCheckSpeedMode">Contact Check Speed Mode</see> or none if unknown. </returns>
    Public Function WriteContactCheckSpeedMode(ByVal value As ContactCheckSpeedMode) As ContactCheckSpeedMode?
        Me.Session.WriteLine("{0}.contact.speed={0}.{1}", Me.SourceMeasureUnitReference, value.ExtractBetween())
        Me.ContactCheckSpeedMode = value
        Return Me.ContactCheckSpeedMode
    End Function

#End Region

#Region " THRESHOLD "

    Private _ContactCheckThreshold As Integer?

    ''' <summary> Gets or sets (Protected) the contact check threshold. </summary>
    ''' <value> The contact check threshold. </value>
    Public Property ContactCheckThreshold As Integer?
        Get
            Return Me._ContactCheckThreshold
        End Get
        Set(ByVal value As Integer?)
            If Not Nullable.Equals(value, Me.ContactCheckThreshold) Then
                Me._ContactCheckThreshold = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Programs and reads back the Contact Check Threshold Level. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The <see cref="ContactCheckThreshold">Contact Check Threshold</see> or nothing if not known. </returns>
    Public Function ApplyContactCheckThreshold(ByVal value As Integer) As Integer?
        Me.WriteContactCheckThreshold(value)
        Return Me.QueryContactCheckThreshold()
    End Function

    ''' <summary> Reads back the Contact Check Threshold Level. </summary>
    ''' <returns> The <see cref="ContactCheckThreshold">Contact Check Threshold</see> or nothing if not known. </returns>
    Public Overridable Function QueryContactCheckThreshold() As Integer?
        Me.ContactCheckThreshold = Me.Session.QueryPrint(Me.ContactCheckThreshold.GetValueOrDefault(15), "{0}.contact.threshold()", Me.SourceMeasureUnitReference)
        Return Me.ContactCheckThreshold
    End Function

    ''' <summary> Programs the Contact Check Threshold Level without updating the value from the device. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The <see cref="ContactCheckThreshold">Contact Check Threshold</see> or nothing if not known. </returns>
    Public Overridable Function WriteContactCheckThreshold(ByVal value As Integer) As Integer?
        Me.Session.WriteLine("{0}.contact.threshold={1}", Me.SourceMeasureUnitReference, value)
        Me.ContactCheckThreshold = value
        Return Me.ContactCheckThreshold
    End Function

#End Region

#Region " RESISTANCES "

    Dim _ContactResistances As String

    ''' <summary> Gets or sets (Protected) the contact resistances. </summary>
    ''' <value> The contact resistances. </value>
    Public Property ContactResistances() As String
        Get
            Return Me._ContactResistances
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.ContactResistances) Then
                Me._ContactResistances = value
                Me.SafePostPropertyChanged()
            End If
        End Set

    End Property

    ''' <summary> Reads the Contact Resistances. </summary>
    ''' <returns> The <see cref="ContactResistances">Contact Resistances</see> or nothing if not known. </returns>
    Public Overridable Function QueryContactResistances() As String
        Me.Session.MakeEmulatedReplyIfEmpty(Me.ContactResistances)
        Me.ContactResistances = Me.Session.QueryPrintTrimEnd("{0}.contact.r()", Me.SourceMeasureUnitReference)
        Return Me.ContactResistances
    End Function

#End Region

#Region " CONTACT CHECK OKAY "

    ''' <summary> ContactCheckOkay. </summary>
    Private _ContactCheckOkay As Boolean?

    ''' <summary> Gets or sets the cached Contact Check Okay sentinel. </summary>
    ''' <value> <c>null</c> if Contact Check Okay is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property ContactCheckOkay As Boolean?
        Get
            Return Me._ContactCheckOkay
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ContactCheckOkay, value) Then
                Me._ContactCheckOkay = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Queries the Contact Check status. Also sets the <see cref="ContactCheckOkay">contact check</see> sentinel. </summary>
    ''' <returns> <c>null</c> if not known; <c>True</c> if ContactCheckOkay; otherwise, <c>False</c>. </returns>
    Public Function QueryContactCheckOkay() As Boolean?
        Me.Session.MakeTrueFalseReplyIfEmpty(True)
        Me.ContactCheckOkay = Me.Session.IsStatementTrue("{0}.contact.check()", Me.SourceMeasureUnitReference)
        Return Me.ContactCheckOkay
    End Function

#Region " CONTACT CHECK "

    ''' <summary> Determines whether contact resistances are below the specified threshold. </summary>
    ''' <exception cref="NativeException"> Thrown when a Visa error condition occurs. </exception>
    ''' <exception cref="DeviceException"> Thrown when a device error condition occurs. </exception>
    ''' <param name="threshold"> The threshold. </param>
    ''' <returns> <c>True</c> if passed, <c>False</c> if failed, <c>True</c> if passed. Exception is
    ''' thrown if failed configuring contact check. </returns>
    Public Function CheckContacts(ByVal threshold As Integer) As Boolean?

        Me.ContactResistances = "-1,-1"
        If Not threshold.Equals(Me.ContactCheckThreshold) Then
            Me.Session.LastNodeNumber = New Integer?
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "writing contact check limit {0};. ", threshold)
            Me.WriteContactCheckThreshold(threshold)
            Me.CheckThrowDeviceException(False, "setting up contact check threshold;. using '{0}'", Me.Session.LastMessageSent)
        End If

        If Not Me.ContactCheckSpeedMode.Equals(Tsp.ContactCheckSpeedMode.Fast) Then
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "writing contact mode {0};. ", Tsp.ContactCheckSpeedMode.Fast)
            Me.WriteContactCheckSpeedMode(Tsp.ContactCheckSpeedMode.Fast)
            Me.CheckThrowDeviceException(False, "setting up contact check speed;. using '{0}'", Me.Session.LastMessageSent)
        End If

        Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "querying contact check;. ")
        Me.QueryContactCheckOkay()
        Me.CheckThrowDeviceException(True, "checking contact;. using '{0}'", Me.Session.LastMessageSent)
        If Me.ContactCheckOkay.HasValue AndAlso Not Me.ContactCheckOkay.Value Then
            Me.Session.LastAction = Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "reading contact check resistance;. ")
            Me.QueryContactResistances()
            Me.CheckThrowDeviceException(True, "reading contacts;. using '{0}'", Me.Session.LastMessageSent)
            If String.IsNullOrWhiteSpace(Me._ContactResistances) Then
                Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                       "Contact check failed;. Failed fetching contact resistances using  '{0}'", Me.Session.LastMessageSent)
            Else
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
                                       "Contact check failed;. Contact resistance {0} exceeded the limit {1}",
                                       Me.ContactResistances, Me.ContactCheckThreshold)
            End If
        End If


        If Me.ContactCheckOkay.HasValue Then
            Return Me.ContactCheckOkay.Value
        Else
            Return New Boolean?
        End If
    End Function

#End Region

#End Region

#End Region

End Class

''' <summary> Specifies the contact check speed modes. </summary>
Public Enum ContactCheckSpeedMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Fast (CONTACT_FAST)")> Fast
    <ComponentModel.Description("Medium (CONTACT_MEDIUM)")> Medium
    <ComponentModel.Description("Slow (CONTACT_SLOW)")> Slow
End Enum
