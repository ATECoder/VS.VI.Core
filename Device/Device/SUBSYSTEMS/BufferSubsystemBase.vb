Imports System.Threading
''' <summary> Defines the contract that must be implemented by a Buffer Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class BufferSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="BufferSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Capacity = 0
    End Sub

#End Region

#Region " COMMANDS "

    ''' <summary> Gets or sets the Clear Buffer command. </summary>
    ''' <value> The ClearBuffer command. </value>
    ''' <remarks> SCPI: ":TRAC:CLE". </remarks>
    Protected Overridable ReadOnly Property ClearBufferCommand As String

    ''' <summary> Clears the buffer. </summary>
    Public Sub ClearBuffer()
        Me.Write(Me.ClearBufferCommand)
    End Sub

#End Region

#Region " Fill Once ENABLED "

    ''' <summary> Fill Once enabled. </summary>
    Private _FillOnceEnabled As Boolean?

    ''' <summary> Gets or sets the cached Fill Once Enabled sentinel. </summary>
    ''' <remarks> When this is enabled, a delay is added before each measurement. </remarks>
    ''' <value>
    ''' <c>null</c> if Fill Once Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>.
    ''' </value>
    Public Property FillOnceEnabled As Boolean?
        Get
            Return Me._FillOnceEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FillOnceEnabled, value) Then
                Me._FillOnceEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Fill Once Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyFillOnceEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteFillOnceEnabled(value)
        Return Me.QueryFillOnceEnabled()
    End Function

    ''' <summary> Gets or sets the automatic Delay enabled query command. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overridable ReadOnly Property FillOnceEnabledQueryCommand As String

    ''' <summary> Queries the Fill Once Enabled sentinel. Also sets the
    ''' <see cref="FillOnceEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryFillOnceEnabled() As Boolean?
        Me.FillOnceEnabled = Me.Query(Me.FillOnceEnabled, Me.FillOnceEnabledQueryCommand)
        Return Me.FillOnceEnabled
    End Function

    ''' <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overridable ReadOnly Property FillOnceEnabledCommandFormat As String

    ''' <summary> Writes the Fill Once Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteFillOnceEnabled(ByVal value As Boolean) As Boolean?
        Me.FillOnceEnabled = Me.Write(value, Me.FillOnceEnabledCommandFormat)
        Return Me.FillOnceEnabled
    End Function

#End Region

#Region " CAPACITY "

    Private _Capacity As Integer?

    ''' <summary> Gets or sets the cached Buffer Capacity. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Buffer model. </remarks>
    ''' <value> The Buffer Capacity or none if not set or unknown. </value>
    Public Overloads Property Capacity As Integer?
        Get
            Return Me._Capacity
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.Capacity, value) Then
                Me._Capacity = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Buffer Capacity. </summary>
    ''' <param name="value"> The current Capacity. </param>
    ''' <returns> The Capacity or none if unknown. </returns>
    Public Function ApplyCapacity(ByVal value As Integer) As Integer?
        Me.WriteCapacity(value)
        Return Me.QueryCapacity()
    End Function

    ''' <summary> Gets or sets the points count query command. </summary>
    ''' <value> The points count query command. </value>
    ''' <remarks> SCPI: ":TRAC:POIN:COUN?" </remarks>
    Protected Overridable ReadOnly Property CapacityQueryCommand As String

    ''' <summary> Queries the current Capacity. </summary>
    ''' <returns> The Capacity or none if unknown. </returns>
    Public Function QueryCapacity() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.CapacityQueryCommand) Then
            Me.Capacity = Me.Session.Query(0I, Me.CapacityQueryCommand)
        End If
        Return Me.Capacity
    End Function

    ''' <summary> Gets or sets the points count command format. </summary>
    ''' <value> The points count query command format. </value>
    ''' <remarks> SCPI: ":TRAC:POIN:COUN {0}" </remarks>
    Protected Overridable ReadOnly Property CapacityCommandFormat As String

    ''' <summary> Write the Buffer Capacity without reading back the value from the device. </summary>
    ''' <param name="value"> The current Capacity. </param>
    ''' <returns> The Capacity or none if unknown. </returns>
    Public Function WriteCapacity(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.CapacityCommandFormat) Then
            Me.Session.WriteLine(Me.CapacityCommandFormat, value)
        End If
        Me.Capacity = value
        Return Me.Capacity
    End Function

#End Region

#Region " ACTUAL POINT COUNT "

    ''' <summary> Number of ActualPoint. </summary>
    Private _ActualPointCount As Integer?

    ''' <summary> Gets or sets the cached Buffer ActualPointCount. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Buffer model. </remarks>
    ''' <value> The Buffer ActualPointCount or none if not set or unknown. </value>
    Public Overloads Property ActualPointCount As Integer?
        Get
            Return Me._ActualPointCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ActualPointCount, value) Then
                Me._ActualPointCount = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the ActualPoint count query command. </summary>
    ''' <value> The ActualPoint count query command. </value>
    ''' <remarks> SCPI: ":TRAC:ACT?" </remarks>
    Protected Overridable ReadOnly Property ActualPointCountQueryCommand As String

    ''' <summary> Queries the current ActualPointCount. </summary>
    ''' <returns> The ActualPointCount or none if unknown. </returns>
    Public Function QueryActualPointCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.ActualPointCountQueryCommand) Then
            Me.ActualPointCount = Me.Session.Query(0I, Me.ActualPointCountQueryCommand)
        End If
        Return Me.ActualPointCount
    End Function

#End Region

#Region " FIRST POINT NUMBER "

    ''' <summary> Number of First Point. </summary>
    Private _FirstPointNumber As Integer?

    ''' <summary> Gets or sets the cached buffer First Point Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Buffer model. </remarks>
    ''' <value> The buffer First Point Number or none if not set or unknown. </value>
    Public Overloads Property FirstPointNumber As Integer?
        Get
            Return Me._FirstPointNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.FirstPointNumber, value) Then
                Me._FirstPointNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets The First Point Number query command. </summary>
    ''' <value> The First Point Number query command. </value>
    ''' <remarks> SCPI: ":TRAC:ACT:STA?" </remarks>
    Protected Overridable ReadOnly Property FirstPointNumberQueryCommand As String

    ''' <summary> Queries the current FirstPointNumber. </summary>
    ''' <returns> The First Point Number or none if unknown. </returns>
    Public Function QueryFirstPointNumber() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.FirstPointNumberQueryCommand) Then
            Me.FirstPointNumber = Me.Session.Query(0I, Me.FirstPointNumberQueryCommand)
        End If
        Return Me.FirstPointNumber
    End Function

#End Region

#Region " LAST POINT NUMBER "

    ''' <summary> Number of Last Point. </summary>
    Private _LastPointNumber As Integer?

    ''' <summary> Gets or sets the cached buffer Last Point Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Buffer model. </remarks>
    ''' <value> The buffer Last Point Number or none if not set or unknown. </value>
    Public Overloads Property LastPointNumber As Integer?
        Get
            Return Me._LastPointNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.LastPointNumber, value) Then
                Me._LastPointNumber = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets The Last Point Number query command. </summary>
    ''' <value> The Last Point Number query command. </value>
    ''' <remarks> SCPI: ":TRAC:ACT:END?" </remarks>
    Protected Overridable ReadOnly Property LastPointNumberQueryCommand As String

    ''' <summary> Queries the current Last Point Number. </summary>
    ''' <returns> The LastPointNumber or none if unknown. </returns>
    Public Function QueryLastPointNumber() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.LastPointNumberQueryCommand) Then
            Me.LastPointNumber = Me.Session.Query(0I, Me.LastPointNumberQueryCommand)
        End If
        Return Me.LastPointNumber
    End Function

#End Region

#Region " DATA "

    ''' <summary> String. </summary>
    Private _Data As String

    ''' <summary> Gets or sets the cached Buffer Data. </summary>
    Public Overloads Property Data As String
        Get
            Return Me._Data
        End Get
        Protected Set(ByVal value As String)
            If Not Nullable.Equals(Me.Data, value) Then
                Me._Data = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the data query command. </summary>
    ''' <value> The points count query command. </value>
    ''' <remarks> SCPI: ":TRAC:DATA?" </remarks>
    Protected Overridable ReadOnly Property DataQueryCommand As String

    ''' <summary> Queries the current Data. </summary>
    ''' <returns> The Data or none if unknown. </returns>
    Public Function QueryData() As String
        If Not String.IsNullOrWhiteSpace(Me.DataQueryCommand) Then
            Me.Session.WriteLine(Me.DataQueryCommand)
            ' read the entire data.
            Me.Data = Me.Session.ReadFreeLineTrimEnd()
        End If
        Return Me.Data
    End Function

    ''' <summary> Queries the current Data. </summary>
    ''' <returns> The Data or empty if none. </returns>
    Public Function QueryData(ByVal queryCommand As String) As String
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            Me.Data = ""
            Me.Session.WriteLine(queryCommand)
            ' read the entire data.
            Me.Data = Me.Session.ReadFreeLineTrimEnd()
        End If
        Return Me.Data
    End Function

#End Region

#Region " BUFFER STREAM "

    ''' <summary> Queries the current Data. </summary>
    ''' <returns> The Data or empty if none. </returns>
    Public Function QueryBufferReadings() As IEnumerable(Of BufferReading)
        Dim count As Integer = Me.QueryActualPointCount().GetValueOrDefault(0)
        If count > 0 Then
            Dim first As Integer = Me.QueryFirstPointNumber().GetValueOrDefault(0)
            Dim last As Integer = Me.QueryLastPointNumber().GetValueOrDefault(0)
            Return Me.QueryBufferReadings(first, last)
        Else
            Return New List(Of BufferReading)
        End If
    End Function

    Public ReadOnly Property DefaultBuffer1ReadCommandFormat As String = ":TRAC:DATA? {0},{1},'defbuffer1',READ,TST,STAT,UNIT"

    ''' <summary> Queries the current Data. </summary>
    ''' <param name="firstIndex"> Zero-based index of the first. </param>
    ''' <param name="lastIndex">  Zero-based index of the last. </param>
    ''' <returns> The Data or empty if none. </returns>
    Public Function QueryBufferReadings(ByVal firstIndex As Integer, ByVal lastIndex As Integer) As IEnumerable(Of BufferReading)
        Me.QueryData(String.Format(Me.DefaultBuffer1ReadCommandFormat, firstIndex, lastIndex))
        Dim q As New Queue(Of String)(Data.Split(","c))
        Dim l As New List(Of BufferReading)
        Do While q.Any
            l.Add(New BufferReading(q))
        Loop
        Return l
    End Function

    ''' <summary> Gets or sets the buffer readings. </summary>
    ''' <value> The buffer readings. </value>
    Public ReadOnly Property BufferReadings As BufferReadingCollection

    ''' <summary> Gets a queue of new buffer readings. </summary>
    ''' <value> A thread safe Queue of buffer readings. </value>
    Public ReadOnly Property NewBufferReadingsQueue As System.Collections.Concurrent.ConcurrentQueue(Of BufferReading)

    ''' <summary> Gets the number of buffer readings. </summary>
    ''' <value> The number of buffer readings. </value>
    Public ReadOnly Property BufferReadingsCount As Integer
        Get
            Return If(Me.BufferReadings?.Any, Me.BufferReadings.Count, 0)
        End Get
    End Property

    ''' <summary> Gets the last buffer reading. </summary>
    ''' <value> The last buffer reading. </value>
    Public ReadOnly Property LastBufferReading As BufferReading
        Get
            Return Me.BufferReadings.LastReading
        End Get
    End Property

    ''' <summary> Gets the items locker. </summary>
    ''' <value> The items locker. </value>
    Protected ReadOnly Property ItemsLocker As New ReaderWriterLockSlim()

    ''' <summary> Enqueue range. </summary>
    ''' <param name="items"> The items. </param>
    Public Sub EnqueueRange(ByVal items As IEnumerable(Of BufferReading))
        If items Is Nothing OrElse Not items.Any Then Return
        Me.ItemsLocker.EnterWriteLock()
        Try
            For Each item As BufferReading In items
                Me.NewBufferReadingsQueue.Enqueue(item)
            Next item
        Finally
            Me.ItemsLocker.ExitWriteLock()
        End Try
    End Sub

    ''' <summary> Enumerates dequeue range in this collection. </summary>
    ''' <returns>
    ''' An enumerator that allows for each to be used to process dequeue range in this collection.
    ''' </returns>
    Public Function DequeueRange() As IEnumerable(Of BufferReading)
        Dim result As New List(Of BufferReading)
        Dim value As BufferReading = Nothing
        Do While Me.NewBufferReadingsQueue.Any
            If Me.NewBufferReadingsQueue.TryDequeue(value) Then
                result.Add(value)
            End If
        Loop
        Return result
    End Function


    ''' <summary> Stream buffer. </summary>
    ''' <param name="triggerSubsystem"> The trigger subsystem. </param>
    ''' <param name="pollPeriod">       The poll period. </param>
    Public Sub StreamBuffer(ByVal triggerSubsystem As TriggerSubsystemBase, ByVal pollPeriod As TimeSpan)
        If triggerSubsystem Is Nothing Then Throw New ArgumentNullException(NameOf(triggerSubsystem))
        Me.ApplyCapturedSyncContext()
        Dim first As Integer = 0
        Dim last As Integer = 0
        Me._BufferReadings = New BufferReadingCollection
        Me._NewBufferReadingsQueue = New Concurrent.ConcurrentQueue(Of BufferReading)
        triggerSubsystem.QueryTriggerState()
        Do While triggerSubsystem.IsTriggerStateActive
            If first = 0 Then first = Me.QueryFirstPointNumber().GetValueOrDefault(0)
            If first > 0 Then
                last = Me.QueryLastPointNumber().GetValueOrDefault(0)
                If (last - first + 1) > Me.BufferReadings.Count Then
                    Dim newReadings As IEnumerable(Of BufferReading) = Me.QueryBufferReadings(Me.BufferReadings.Count + 1, last)
                    Me.BufferReadings.Add(newReadings)
                    Me.EnqueueRange(newReadings)
                    Me.SafePostPropertyChanged(NameOf(VI.BufferSubsystemBase.BufferReadingsCount))
                End If
            End If
            Threading.Thread.Sleep(pollPeriod)
            Windows.Forms.Application.DoEvents()
            triggerSubsystem.QueryTriggerState()
        Loop
    End Sub

    ''' <summary> Asynchronous stream buffer. </summary>
    ''' <param name="syncContext">      Context for the synchronization. </param>
    ''' <param name="triggerSubsystem"> The trigger subsystem. </param>
    ''' <param name="pollPeriod">       The poll period. </param>
    ''' <returns> A Threading.Tasks.Task. </returns>
    Public Async Function StreamBufferAsync(ByVal syncContext As Threading.SynchronizationContext,
                                            ByVal triggerSubsystem As TriggerSubsystemBase, ByVal pollPeriod As TimeSpan) As Threading.Tasks.Task
        If triggerSubsystem Is Nothing Then Throw New ArgumentNullException(NameOf(triggerSubsystem))
        Me.CaptureSyncContext(syncContext)
        Await Threading.Tasks.Task.Run(Sub() Me.StreamBuffer(triggerSubsystem, pollPeriod))
    End Function

#End Region

End Class

