Imports isr.Core.Pith
Imports isr.Core.Pith.ExceptionExtensions
''' <summary> Defines the contract that must be implemented by Subsystems. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
Public MustInherit Class SubsystemBase
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher, ITalker

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SubsystemBase" /> class. </summary>
    ''' <param name="visaSession"> A reference to a <see cref="VI.SessionBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal visaSession As VI.SessionBase)
        MyBase.New()
        Me._ApplySession(visaSession)
        Me._Talker = New TraceMessageTalker
    End Sub

    ''' <summary> Cleans up unmanaged or managed and unmanaged resources. </summary>
    ''' <param name="disposing"> <c>True</c> if this method releases both managed and unmanaged resources;
    ''' False if this method releases only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me.Talker?.Listeners.Clear()
                Me._Talker = Nothing
                Me._Session = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overridable Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Overridable Sub InitKnownState() Implements IPresettable.InitKnownState
    End Sub

    ''' <summary> Gets the preset command. </summary>
    ''' <value> The preset command. </value>
    Protected Overridable ReadOnly Property PresetCommand As String = ""

    ''' <summary> Sets the subsystem to its preset state. </summary>
    Public Overridable Sub PresetKnownState() Implements IPresettable.PresetKnownState
        If Not String.IsNullOrWhiteSpace(Me.PresetCommand) Then Me.Write(Me.PresetCommand)
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overridable Sub ResetKnownState() Implements IPresettable.ResetKnownState
        If Me.Session Is Nothing Then Throw New InvalidOperationException("Subsystem must have a valid session instance")
    End Sub

#End Region

#Region " SESSION "

    ''' <summary> Applies the session described by value. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub ApplySession(ByVal value As SessionBase)
        Me._ApplySession(value)
        Me.SafePostPropertyChanged(NameOf(Me.ResourceName))
    End Sub

    ''' <summary> Applies the session described by value. </summary>
    ''' <param name="value"> The value. </param>
    Private Sub _ApplySession(ByVal value As SessionBase)
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        Me._Session = value
        If Me._Session.IsDeviceOpen Then
            Me._ResourceName = Me.Session.ResourceName
        Else
            Me._ResourceName = DeviceBase.ResourceNameClosed
        End If
    End Sub

    ''' <summary> Gets the session. </summary>
    ''' <value> The session. </value>
    Public ReadOnly Property Session As SessionBase

    Private _ResourceName As String
    ''' <summary> Gets the name of the resource. </summary>
    ''' <value> The name of the resource or &lt;closed&gt; if not open. </value>
    Public Property ResourceName As String
        Get
            If Me._Session.IsDeviceOpen AndAlso (String.IsNullOrWhiteSpace(Me._ResourceName) OrElse
                  Me._ResourceName.StartsWith(DeviceBase.ResourceNameClosed, StringComparison.OrdinalIgnoreCase)) Then
                Me.ResourceName = Me.Session.ResourceName
            ElseIf Not Me._Session.IsDeviceOpen AndAlso Not String.IsNullOrWhiteSpace(Me._ResourceName) Then
                Me.ResourceName = DeviceBase.ResourceNameClosed
            End If
            Return Me._ResourceName
        End Get
        Set(ByVal value As String)
            Me._ResourceName = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

#End Region

#Region " QUERY / WRITE / EXECUTE "

#Region " ENUMERATION "

    ''' <summary> Issues the query command and parses the returned enum value into an Enum. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The parsed value or none if unknown. </returns>
    Public Function QueryValue(Of T As Structure)(ByVal queryCommand As String, ByVal value As Nullable(Of T)) As Nullable(Of T)
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            Return Me.Session.QueryEnumValue(Of T)(value, queryCommand)
        Else
            Return value
        End If
    End Function

    ''' <summary> Writes the Enum value without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format. </param>
    ''' <returns> The value or none if unknown. </returns>
    Public Function WriteValue(Of T As Structure)(ByVal commandFormat As String, ByVal value As T) As Nullable(Of T)
        Return Me.Session.WriteEnumValue(Of T)(value, commandFormat)
    End Function

    ''' <summary> Issues the query command and parses the returned en um value name into an Enum. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The parsed value or none if unknown. </returns>
    Public Function Query(Of T As Structure)(ByVal queryCommand As String, ByVal value As Nullable(Of T)) As Nullable(Of T)
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            Return Me.Session.QueryEnum(Of T)(value, queryCommand)
        Else
            Return value
        End If
    End Function

    ''' <summary> Writes the Enum value name without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format. </param>
    ''' <returns> The value or none if unknown. </returns>
    Public Function Write(Of T As Structure)(ByVal commandFormat As String, ByVal value As T) As Nullable(Of T)
        Return Me.Session.Write(Of T)(value, commandFormat)
    End Function

#End Region

#Region " BOOLEAN "

    ''' <summary> Queries a <see cref="T:Boolean">Boolean</see> value. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The value. </returns>
    Public Function Query(ByVal value As Boolean?, ByVal queryCommand As String) As Boolean?
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            value = Me.Session.Query(value.GetValueOrDefault(True), queryCommand)
        End If
        Return value
    End Function

    ''' <summary> Write the value without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format. </param>
    ''' <returns> The value. </returns>
    Public Function Write(ByVal value As Boolean, ByVal commandFormat As String) As Boolean?
        If Not String.IsNullOrWhiteSpace(commandFormat) Then
            Me.Session.WriteLine(commandFormat, value.GetHashCode)
        End If
        Return value
    End Function

#End Region

#Region " INTEGER "

    ''' <summary> Queries an <see cref="T:Integer">integer</see> value. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The value. </returns>
    Public Function Query(ByVal value As Integer?, ByVal queryCommand As String) As Integer?
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            value = Me.Session.Query(value.GetValueOrDefault(0), queryCommand)
        End If
        Return value
    End Function

    ''' <summary> Write the value without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format. </param>
    ''' <returns> The value. </returns>
    Public Function Write(ByVal value As Integer, ByVal commandFormat As String) As Integer?
        If Not String.IsNullOrWhiteSpace(commandFormat) Then
            Me.Session.WriteLine(commandFormat, value)
        End If
        Return value
    End Function

#End Region

#Region " DOUBLE "

    ''' <summary> Queries an <see cref="T:Double">Double</see> value. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The value. </returns>
    Public Function Query(ByVal value As Double?, ByVal queryCommand As String) As Double?
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            value = Me.Session.Query(value.GetValueOrDefault(0), queryCommand)
        End If
        Return value
    End Function

    ''' <summary> Write the value without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format. </param>
    ''' <returns> The value. </returns>
    Public Function Write(ByVal value As Double, ByVal commandFormat As String) As Double?
        If Not String.IsNullOrWhiteSpace(commandFormat) Then
            If value >= (Scpi.Syntax.Infinity - 1) Then
                Me.Session.WriteLine(commandFormat, "MAX")
                value = Scpi.Syntax.Infinity
            ElseIf value <= (Scpi.Syntax.NegativeInfinity + 1) Then
                Me.Session.WriteLine(commandFormat, "MIN")
                value = Scpi.Syntax.NegativeInfinity
            Else
                Me.Session.WriteLine(commandFormat, value)
            End If
        End If
        Return value
    End Function

#End Region

#Region " STRING "

    ''' <summary> Queries a <see cref="T:String">String</see> value. </summary>
    ''' <param name="value">        The present value. </param>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The value. </returns>
    Public Function Query(ByVal value As String, ByVal queryCommand As String) As String
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            Return Me.Session.QueryTrimEnd(queryCommand)
        Else
            Return value
        End If
    End Function

    ''' <summary> Write the value without reading back the value from the device. </summary>
    ''' <param name="commandFormat"> The command format. </param>
    ''' <param name="args">          A variable-length parameters list containing arguments. </param>
    ''' <returns> The value. </returns>
    Public Function Write(ByVal commandFormat As String, ByVal ParamArray args() As Object) As String
        Return Me.Write(String.Format(commandFormat, args))
    End Function

    ''' <summary> Write the value without reading back the value from the device. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> The value. </returns>
    Public Function Write(ByVal value As String) As String
        If Not String.IsNullOrWhiteSpace(value) Then
            Me.Session.WriteLine(value)
        End If
        Return value
    End Function

#End Region

#Region " TIME SPAN "

    ''' <summary> Queries an <see cref="T:TimeSpan">TimeSpan</see> value. </summary>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The value. </returns>
    Public Function Query(ByVal value As TimeSpan?, ByVal format As String, ByVal queryCommand As String) As TimeSpan?
        If Not String.IsNullOrWhiteSpace(format) AndAlso Not String.IsNullOrWhiteSpace(queryCommand) Then
            value = Me.Session.Query(format, queryCommand)
        End If
        Return value
    End Function

    ''' <summary> Write the value without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format. </param>
    ''' <returns> The value. </returns>
    Public Function Write(ByVal value As TimeSpan, ByVal commandFormat As String) As TimeSpan?
        If Not String.IsNullOrWhiteSpace(commandFormat) Then
            Me.Session.WriteLine(commandFormat, value)
        End If
        Return value
    End Function

#End Region

#End Region

#Region " TALKER "

    ''' <summary> Gets the trace message talker. </summary>
    ''' <value> The trace message talker. </value>
    Public ReadOnly Property Talker As ITraceMessageTalker

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener)) Implements ITalker.AddListeners
        Me.Talker.AddListeners(listeners)
    End Sub

    ''' <summary> Clears the listeners. </summary>
    Public Sub ClearListeners() Implements ITalker.ClearListeners
        Me.Talker.Listeners.Clear()
    End Sub

    Public Sub AddListeners(talker As ITraceMessageTalker) Implements ITalker.AddListeners
        Me.Talker.AddListeners(talker)
    End Sub

    ''' <summary> Updates the trace log level described by traceLevel. </summary>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overridable Sub UpdateTraceLogLevel(ByVal traceLevel As TraceEventType) Implements ITalker.UpdateTraceLogLevel
        Me.Talker.UpdateTraceLogLevel(traceLevel)
    End Sub

    ''' <summary> Updates the trace show level described by traceLevel. </summary>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overridable Sub UpdateTraceShowLevel(ByVal traceLevel As TraceEventType) Implements ITalker.UpdateTraceShowLevel
        Me.Talker.UpdateTraceShowLevel(traceLevel)
    End Sub

    ''' <summary> Updates the trace log and show level described by the talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Sub UpdateTraceLevels(ByVal talker As ITraceMessageTalker) Implements ITalker.UpdateTraceLevels
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        Me.UpdateTraceLogLevel(talker.TraceLogLevel)
        Me.UpdateTraceShowLevel(talker.TraceShowLevel)
    End Sub

#End Region

End Class

''' <summary> Collection of subsystems. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="12/21/2015" by="David" revision=""> Created. </history>
Public Class SubsystemCollection
    Inherits Collections.ObjectModel.Collection(Of SubsystemBase)
    Implements IPresettablePublisher, ITalker

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Clears the queues and resets all registers to zero. Sets the subsystem properties to
    ''' the following CLS default values:<para>
    ''' </para> </summary>
    Public Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
        For Each element As IPresettable In Me.Items
            element.ClearExecutionState()
        Next
    End Sub

    ''' <summary> Performs a reset and additional custom setting for the subsystem:<para>
    ''' </para> </summary>
    Public Sub InitKnownState() Implements IPresettable.InitKnownState
        For Each element As IPresettable In Me.Items
            element.InitKnownState()
        Next
    End Sub

    ''' <summary> Gets subsystem to the following default system preset values:<para>
    ''' </para> </summary>
    Public Sub PresetKnownState() Implements IPresettable.PresetKnownState
        For Each element As IPresettable In Me.Items
            element.PresetKnownState()
        Next
    End Sub

    ''' <summary> Restore member properties to the following RST or System Preset values:<para>
    ''' </para> </summary>
    Public Sub ResetKnownState() Implements IPresettable.ResetKnownState
        For Each element As IPresettable In Me.Items
            element.ResetKnownState()
        Next
    End Sub

#End Region

#Region " CLEAR/DISPOSE "

    ''' <summary> Dispose items. </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Sub DisposeItems()
        Me.ClearListeners()
        For Each element As IDisposable In Me.Items
            Try
                element.Dispose()
            Catch ex As Exception
                Debug.Assert(Not Debugger.IsAttached, ex.ToFullBlownString)
            End Try
        Next
        Me.Clear()
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Gets or sets the publishable sentinel. </summary>
    ''' <value> The publishable. </value>
    Public Property Publishable As Boolean Implements IPublisher.Publishable

    ''' <summary> Publishes all values. </summary>
    Public Sub Publish() Implements IPublisher.Publish
        For Each element As IPublisher In Me.Items
            element.Publish()
        Next
    End Sub

    ''' <summary> Resume property events. </summary>
    Public Sub ResumePublishing() Implements IPublisher.ResumePublishing
        For Each element As IPublisher In Me.Items
            element.ResumePublishing()
            Me.Publishable = element.Publishable
        Next
    End Sub

    ''' <summary> Suspend publishing. </summary>
    Public Sub SuspendPublishing() Implements IPublisher.SuspendPublishing
        For Each element As IPublisher In Me.Items
            element.SuspendPublishing()
            Me.Publishable = element.Publishable
        Next
    End Sub

    ''' <summary> Capture synchronization context. </summary>
    ''' <param name="syncContext"> Context for the synchronization. </param>
    Public Sub CaptureSyncContext(ByVal syncContext As Threading.SynchronizationContext)
        For Each ss As SubsystemBase In Me
            ss.CaptureSyncContext(syncContext)
        Next
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Adds the listeners. </summary>
    ''' <param name="listeners"> The listeners. </param>
    Public Overridable Sub AddListeners(ByVal listeners As IEnumerable(Of ITraceMessageListener)) Implements ITalker.AddListeners
        For Each element As ITalker In Me.Items
            element.AddListeners(listeners)
        Next
    End Sub

    ''' <summary> Clears the listeners. </summary>
    Public Sub ClearListeners() Implements ITalker.ClearListeners
        For Each element As ITalker In Me.Items
            element.ClearListeners()
        Next
    End Sub

    ''' <summary> Adds the listeners such as the top level trace messages box and log. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Sub AddListeners(talker As ITraceMessageTalker) Implements ITalker.AddListeners
        For Each element As ITalker In Me.Items
            element.AddListeners(talker)
        Next
    End Sub

    ''' <summary> Updates the trace log level described by traceLevel. </summary>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overridable Sub UpdateTraceLogLevel(ByVal traceLevel As TraceEventType) Implements ITalker.UpdateTraceLogLevel
        For Each element As ITalker In Me.Items
            element.UpdateTraceLogLevel(traceLevel)
        Next
    End Sub

    ''' <summary> Updates the trace show level described by traceLevel. </summary>
    ''' <param name="traceLevel"> The trace level. </param>
    Public Overridable Sub UpdateTraceShowLevel(ByVal traceLevel As TraceEventType) Implements ITalker.UpdateTraceShowLevel
        For Each element As ITalker In Me.Items
            element.UpdateTraceShowLevel(traceLevel)
        Next
    End Sub

    ''' <summary> Updates the trace log and show level described by the talker. </summary>
    ''' <param name="talker"> The talker. </param>
    Public Sub UpdateTraceLevels(ByVal talker As ITraceMessageTalker) Implements ITalker.UpdateTraceLevels
        If talker Is Nothing Then Throw New ArgumentNullException(NameOf(talker))
        Me.UpdateTraceLogLevel(talker.TraceLogLevel)
        Me.UpdateTraceShowLevel(talker.TraceShowLevel)
    End Sub

#End Region

End Class

