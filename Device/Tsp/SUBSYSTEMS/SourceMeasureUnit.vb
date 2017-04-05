Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Manages TSP SMU subsystem. </summary>
''' <license> (c) 2007 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/7/2013" by="David" revision="">                  Uses new core. </history>
''' <history date="01/28/2008" by="David" revision="2.0.2949.x">  Use .NET Framework. </history>
''' <history date="03/12/2007" by="David" revision="1.15.2627.x"> Created. </history>
Public MustInherit Class SourceMeasureUnit
    Inherits SourceMeasureUnitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceMeasureUnit" /> class. </summary>
    ''' <remarks> Note that the local node status clear command only clears the SMU status.  So, issue
    ''' a CLS and RST as necessary when adding an SMU. </remarks>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystem">TSP status
    ''' Subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As StatusSubsystemBase)
        Me.New(statusSubsystem, 0, TspSyntax.SourceMeasureUnitNumberA)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="SourceMeasureUnit" /> class. </summary>
    ''' <remarks> Note that the local node status clear command only clears the SMU status.  So, issue
    ''' a CLS and RST as necessary when adding an SMU. </remarks>
    ''' <param name="statusSubsystem"> A reference to a <see cref="statusSubsystem">TSP status
    ''' Subsystem</see>. </param>
    ''' <param name="nodeNumber">      Specifies the node number. </param>
    ''' <param name="smuNumber">       Specifies the SMU (either 'a' or 'b'. </param>
    Protected Sub New(ByVal statusSubsystem As StatusSubsystemBase, ByVal nodeNumber As Integer, ByVal smuNumber As String)
        MyBase.New(statusSubsystem, nodeNumber, smuNumber)
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
        Me.SourceFunction = SourceFunctionMode.VoltageDC
        Me.SenseMode = SenseActionMode.Local
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

    Dim _contactResistances As String

    ''' <summary> Gets or sets (Protected) the contact resistances. </summary>
    ''' <value> The contact resistances. </value>
    Public Property ContactResistances() As String
        Get
            Return Me._contactResistances
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not value.Equals(Me.ContactResistances) Then
                Me._contactResistances = value
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
            Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "writing contact check limit {0};. ", threshold)
            Me.WriteContactCheckThreshold(threshold)
            Me.CheckThrowDeviceException(False, "setting up contact check threshold;. using '{0}'", Me.Session.LastMessageSent)
        End If

        If Not Me.ContactCheckSpeedMode.Equals(Tsp.ContactCheckSpeedMode.Fast) Then
            Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                                                       "writing contact mode {0};. ", Tsp.ContactCheckSpeedMode.Fast)
            Me.WriteContactCheckSpeedMode(Tsp.ContactCheckSpeedMode.Fast)
            Me.CheckThrowDeviceException(False, "setting up contact check speed;. using '{0}'", Me.Session.LastMessageSent)
        End If

        Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "querying contact check;. ")
        Me.QueryContactCheckOkay()
        Me.CheckThrowDeviceException(True, "checking contact;. using '{0}'", Me.Session.LastMessageSent)
        If Me.ContactCheckOkay.HasValue AndAlso Not Me.ContactCheckOkay.Value Then
            Me.Session.LastAction = Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, "reading contact check resistance;. ")
            Me.QueryContactResistances()
            Me.CheckThrowDeviceException(True, "reading contacts;. using '{0}'", Me.Session.LastMessageSent)
            If String.IsNullOrWhiteSpace(Me._contactResistances) Then
                Me.Talker?.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                       "Contact check failed;. Failed fetching contact resistances using  '{0}'", Me.Session.LastMessageSent)
            Else
                Me.Talker?.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId,
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

#Region " SOURCE FUNCTION "

    ''' <summary> The Source Function. </summary>
    Private _SourceFunction As SourceFunctionMode?

    ''' <summary> Gets or sets the cached Source Function. </summary>
    ''' <value> The <see cref="SourceFunctionMode">Source Function</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property SourceFunction As SourceFunctionMode?
        Get
            Return Me._SourceFunction
        End Get
        Protected Set(ByVal value As SourceFunctionMode?)
            If Not Nullable.Equals(Me.SourceFunction, value) Then
                Me._SourceFunction = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Source Function. </summary>
    ''' <param name="value"> The  Source Function. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function</see> or none if unknown. </returns>
    Public Function ApplySourceFunction(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.WriteSourceFunction(value)
        Return Me.QuerySourceFunction()
    End Function

    ''' <summary> Queries the Source Function. </summary>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function</see> or none if unknown. </returns>
    Public Function QuerySourceFunction() As SourceFunctionMode?
        Dim currentValue As String = Me.SourceFunction.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(currentValue)
        currentValue = Me.Session.QueryPrintTrimEnd("{0}.source.func", Me.SourceMeasureUnitReference)
        If String.IsNullOrWhiteSpace(currentValue) Then
            Dim message As String = "Failed fetching Source Function"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.SourceFunction = New SourceFunctionMode?
        Else
            Dim se As New StringEnumerator(Of SourceFunctionMode)
            Me.SourceFunction = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.SourceFunction
    End Function

    ''' <summary> Writes the Source Function without reading back the value from the device. </summary>
    ''' <param name="value"> The Source Function. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function</see> or none if unknown. </returns>
    Public Function WriteSourceFunction(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.Session.WriteLine("{0}.source.func = {0}.{1}", Me.SourceMeasureUnitReference, value.ExtractBetween())
        Me.SourceFunction = value
        Return Me.SourceFunction
    End Function

#End Region

#Region " SENSE MODE "

    ''' <summary> The Sense Action. </summary>
    Private _SenseMode As SenseActionMode?

    ''' <summary> Gets or sets the cached Sense Action. </summary>
    ''' <value> The <see cref="SenseActionMode">Sense Action</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property SenseMode As SenseActionMode?
        Get
            Return Me._SenseMode
        End Get
        Protected Set(ByVal value As SenseActionMode?)
            If Not Nullable.Equals(Me.SenseMode, value) Then
                Me._SenseMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sense Action. </summary>
    ''' <param name="value"> The  Sense Action. </param>
    ''' <returns> The <see cref="SenseActionMode">Sense Action</see> or none if unknown. </returns>
    Public Function ApplySenseMode(ByVal value As SenseActionMode) As SenseActionMode?
        Me.WriteSenseMode(value)
        Return Me.QuerySenseMode()
    End Function

    ''' <summary> Queries the Sense Action. </summary>
    ''' <returns> The <see cref="SenseActionMode">Sense Action</see> or none if unknown. </returns>
    Public Function QuerySenseMode() As SenseActionMode?
        Dim currentValue As String = Me.SenseMode.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(currentValue)
        currentValue = Me.Session.QueryPrintTrimEnd("{0}.sense", Me.SourceMeasureUnitReference)
        If String.IsNullOrWhiteSpace(currentValue) Then
            Dim message As String = "Failed fetching Sense Action"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.SenseMode = New SenseActionMode?
        Else
            Dim se As New StringEnumerator(Of SenseActionMode)
            Me.SenseMode = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.SenseMode
    End Function

    ''' <summary> Writes the Sense Action without reading back the value from the device. </summary>
    ''' <param name="value"> The Sense Action. </param>
    ''' <returns> The <see cref="SenseActionMode">Sense Action</see> or none if unknown. </returns>
    Public Function WriteSenseMode(ByVal value As SenseActionMode) As SenseActionMode?
        Me.Session.WriteLine("{0}.sense = {0}.{1}", Me.SourceMeasureUnitReference, value.ExtractBetween())
        Me.SenseMode = value
        Return Me.SenseMode
    End Function

#End Region

End Class

''' <summary> Specifies the source function modes. </summary>
Public Enum SourceFunctionMode
    <Description("None")> None
    <Description("DC Voltage (OUTPUT_DCVOLTS)")> VoltageDC
    <Description("DC Current (OUTPUT_DCAMPS)")> CurrentDC
End Enum

''' <summary> Specifies the sense modes. </summary>
Public Enum SenseActionMode
    <Description("None")> None
    <Description("Remote (SENSE_REMOTE)")> Remote
    <Description("Local (SENSE_LOCAL)")> Local
End Enum

''' <summary> Specifies the contact check speed modes. </summary>
Public Enum ContactCheckSpeedMode
    <ComponentModel.Description("None")> None
    <ComponentModel.Description("Fast (CONTACT_FAST)")> Fast
    <ComponentModel.Description("Medium (CONTACT_MEDIUM)")> Medium
    <ComponentModel.Description("Slow (CONTACT_SLOW)")> Slow
End Enum
