Imports isr.Core.Pith
Imports isr.Core.Pith.StopwatchExtensions
Imports isr.VI.National.Visa
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
        Me.ConstructorSafeTalkerSetter(New TraceMessageTalker)
        Me._PresetRefractoryPeriod = TimeSpan.FromMilliseconds(100)
    End Sub

    ''' <summary> Cleans up unmanaged or managed and unmanaged resources. </summary>
    ''' <param name="disposing"> <c>True</c> if this method releases both managed and unmanaged resources;
    ''' False if this method releases only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me.Talker = Nothing
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
        If Not String.IsNullOrWhiteSpace(Me.PresetCommand) Then
            Me.Write(Me.PresetCommand)
            If Me.Session.IsSessionOpen Then Stopwatch.StartNew.Wait(Me.PresetRefractoryPeriod)
        End If
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overridable Sub ResetKnownState() Implements IPresettable.ResetKnownState
        If Me.Session Is Nothing Then Throw New InvalidOperationException("Subsystem must have a valid session instance")
    End Sub

    Private _PresetRefractoryPeriod As TimeSpan
    ''' <summary> Gets the Preset refractory period. </summary>
    ''' <value> The Preset refractory period. </value>
    Public Property PresetRefractoryPeriod As TimeSpan
        Get
            Return Me._PresetRefractoryPeriod
        End Get
        Set(value As TimeSpan)
            If Not value.Equals(Me.PresetRefractoryPeriod) Then
                Me._PresetRefractoryPeriod = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property


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

    ''' <summary> Queries first value returned from the instrument. </summary>
    ''' <param name="queryCommand"> The query command. </param>
    ''' <param name="value">        The value. </param>
    ''' <returns> The first value. </returns>
    Public Function QueryFirstValue(Of T As Structure)(ByVal queryCommand As String, ByVal value As Nullable(Of T)) As Nullable(Of T)
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            Return Me.Session.QueryFirstEnumValue(Of T)(value, queryCommand)
        Else
            Return value
        End If
    End Function

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

    ''' <summary> Issues the query command and parses the returned enum value name into an Enum. </summary>
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

    ''' <summary> Queries and parses the second value from the instrument. </summary>
    ''' <param name="value">        The value. </param> 
    ''' <param name="queryCommand"> The query command. </param>
    ''' <returns> The second. </returns>
    Public Function QuerySecond(ByVal value As Double?, ByVal queryCommand As String) As Double?
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            value = Me.Session.QuerySecond(value.GetValueOrDefault(0), queryCommand)
        End If
        Return value
    End Function

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

#Region " PAYLOAD "

    ''' <summary>
    ''' Issues the query command and parses the returned payload.
    ''' </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="payload"> The payload. </param>
    ''' <returns> <c>True</c> if <see cref="PayloadStatus"/> is <see cref="PayloadStatus.Okay"/>; otherwise <c>False</c>. </returns>
    Public Function Query(ByVal payload As PayloadBase) As Boolean
        If payload Is Nothing Then Throw New ArgumentNullException(NameOf(payload))
        Dim result As Boolean = True
        If Not String.IsNullOrWhiteSpace(payload.QueryCommand) Then
            result = Me.Session.Query(payload)
        End If
        Return result
    End Function

    ''' <summary> Write the payload. A <see cref="Query(PayloadBase)"/> must be issued to get the value from the device. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="payload"> The payload. </param>
    ''' <returns> <c>True</c> if <see cref="PayloadStatus"/> is <see cref="PayloadStatus.Okay"/>; otherwise <c>False</c>. </returns>
    Public Function Write(ByVal payload As PayloadBase) As Boolean
        If payload Is Nothing Then Throw New ArgumentNullException(NameOf(payload))
        Dim result As Boolean = True
        If Not String.IsNullOrWhiteSpace(payload.CommandFormat) Then
            result = Me.Session.Write(payload)
        End If
        Return result
    End Function

#End Region

#End Region

End Class

