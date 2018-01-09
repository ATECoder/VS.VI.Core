Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a Sense Channel Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific ReSenses, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class SenseChannelSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    ''' <param name="channelNumber">   The channel number. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.ChannelNumber = channelNumber
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

#End Region

#Region " COMMANDS "

    ''' <summary> Gets the clear command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:CLE" </remarks>
    ''' <value> The clear compensations command. </value>
    Protected Overridable ReadOnly Property ClearCompensationsCommand As String

    ''' <summary> Clears the Compensations. </summary>
    Public Sub ClearCompensations()
        Me.Write(Me.ClearCompensationsCommand)
    End Sub


#End Region

#Region " ADAPTER TYPE "

    ''' <summary> List adapters. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub ListAdapters(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Dim selectedIndex As Integer = listControl.SelectedIndex
        With listControl
            .DataSource = Nothing
            .Items.Clear()
            .DataSource = GetType(AdapterType).ValueDescriptionPairs(Me.SupportedAdapterTypes)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            If .Items.Count > 0 Then
                .SelectedIndex = Math.Max(selectedIndex, 0)
            End If
        End With
    End Sub

    ''' <summary> Returns the function mode selected by the list control. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    ''' <returns> The SenseAdapterType. </returns>
    Public Shared Function SelectedAdapterType(ByVal listControl As Windows.Forms.ComboBox) As AdapterType
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        Return CType(CType(listControl.SelectedItem, System.Collections.Generic.KeyValuePair(Of [Enum], String)).Key, AdapterType)
    End Function

    ''' <summary> Safe select function mode. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="listControl"> The list control. </param>
    Public Sub SafeSelectAdapterType(ByVal listControl As Windows.Forms.ComboBox)
        If listControl Is Nothing Then Throw New ArgumentNullException(NameOf(listControl))
        If Me.AdapterType.HasValue Then
            listControl.SafeSelectItem(Me.AdapterType.Value, Me.AdapterType.Value.Description)
        End If
    End Sub

    Private _SupportedAdapterTypes As AdapterType
    ''' <summary>
    ''' Gets or sets the supported Adapter Type.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedAdapterTypes() As AdapterType
        Get
            Return _SupportedAdapterTypes
        End Get
        Set(ByVal value As AdapterType)
            If Not Me.SupportedAdapterTypes.Equals(value) Then
                Me._SupportedAdapterTypes = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property



    ''' <summary> The Adapter Type. </summary>
    Private _AdapterType As AdapterType?

    ''' <summary> Gets or sets the cached Adapter Type. </summary>
    ''' <value> The <see cref="AdapterType">Adapter Type</see> or none if not set or
    ''' unknown. </value>
    Public Overridable Property AdapterType As AdapterType?
        Get
            Return Me._AdapterType
        End Get
        Protected Set(ByVal value As AdapterType?)
            If Not Nullable.Equals(Me.AdapterType, value) Then
                Me._AdapterType = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Adapter Type. </summary>
    ''' <param name="value"> The Adapter Type. </param>
    ''' <returns> The <see cref="AdapterType">Adapter Type</see> or none if unknown. </returns>
    Public Function ApplyAdapterType(ByVal value As AdapterType) As AdapterType?
        Me.WriteAdapterType(value)
        Return Me.QueryAdapterType()
    End Function

    ''' <summary> Gets or sets the Adapter Type query command. </summary>
    ''' <value> The Adapter Type query command, e.g., :SENS:ADAPT? </value>
    Protected Overridable ReadOnly Property AdapterTypeQueryCommand As String

    ''' <summary> Queries the Adapter Type. </summary>
    ''' <returns> The <see cref="AdapterType">Adapter Type</see> or none if unknown. </returns>
    Public Function QueryAdapterType() As AdapterType?
        If String.IsNullOrWhiteSpace(Me.AdapterTypeQueryCommand) Then
            Me.AdapterType = New AdapterType?
        Else
            Dim mode As String = Me.AdapterType.ToString
            Me.Session.MakeEmulatedReplyIfEmpty(mode)
            mode = Me.Session.QueryTrimEnd(Me.AdapterTypeQueryCommand)
            If String.IsNullOrWhiteSpace(mode) Then
                Dim message As String = "Failed fetching Adapter Type"
                Debug.Assert(Not Debugger.IsAttached, message)
                Me.AdapterType = New AdapterType?
            Else
                Dim se As New StringEnumerator(Of AdapterType)
                Me.AdapterType = se.ParseContained(mode.BuildDelimitedValue)
            End If
        End If
        Return Me.AdapterType
    End Function

    ''' <summary> Gets or sets the Adapter Type command. </summary>
    ''' <value> The Adapter Type command, e.g., :SENS:ADAPT {0}. </value>
    Protected Overridable ReadOnly Property AdapterTypeCommandFormat As String

    ''' <summary> Writes the Adapter Type without reading back the value from the device. </summary>
    ''' <param name="value"> The Adapter Type. </param>
    ''' <returns> The <see cref="AdapterType">Adapter Type</see> or none if unknown. </returns>
    Public Function WriteAdapterType(ByVal value As AdapterType) As AdapterType?
        If Not String.IsNullOrWhiteSpace(Me.AdapterTypeCommandFormat) Then
            Me.Session.WriteLine(Me.AdapterTypeCommandFormat, value.ExtractBetween())
        End If
        Me.AdapterType = value
        Return Me.AdapterType
    End Function

#End Region

#Region " APERTURE (NPLC) "

    ''' <summary> The Range of the Aperture. </summary>
    Public Property ApertureRange As Core.Pith.RangeR

    ''' <summary> The Aperture. </summary>
    Private _Aperture As Double?

    ''' <summary> Gets the integration period. </summary>
    ''' <value> The integration period. </value>
    Public ReadOnly Property IntegrationPeriod As TimeSpan?
        Get
            If Me.Aperture.HasValue Then
                Return VI.StatusSubsystemBase.FromPowerLineCycles(Me.Aperture.Value)
            Else
                Return New TimeSpan?
            End If
        End Get
    End Property

    ''' <summary> Gets or sets the cached sense Aperture. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Aperture As Double?
        Get
            Return Me._Aperture
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Aperture, value) Then
                Me._Aperture = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the sense Aperture. </summary>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> The Aperture. </returns>
    Public Function ApplyAperture(ByVal value As Double) As Double?
        Me.WriteAperture(value)
        Return Me.QueryAperture
    End Function

    ''' <summary> Gets or sets The Aperture query command. </summary>
    ''' <value> The Aperture query command. </value>
    Protected Overridable ReadOnly Property ApertureQueryCommand As String

    ''' <summary> Queries The Aperture. </summary>
    ''' <returns> The Aperture or none if unknown. </returns>
    Public Function QueryAperture() As Double?
        Me.Aperture = Me.Query(Me.Aperture, Me.ApertureQueryCommand)
        Return Me.Aperture
    End Function

    ''' <summary> Gets or sets The Aperture command format. </summary>
    ''' <value> The Aperture command format. </value>
    Protected Overridable ReadOnly Property ApertureCommandFormat As String

    ''' <summary> Writes The Aperture without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Aperture. </remarks>
    ''' <param name="value"> The Aperture. </param>
    ''' <returns> The Aperture. </returns>
    Public Function WriteAperture(ByVal value As Double) As Double?
        Me.Aperture = Me.Write(value, Me.ApertureCommandFormat)
        Return Me.Aperture
    End Function

#End Region

#Region " FREQUENCY POINTS TYPE "

    ''' <summary> The Frequency Points Type. </summary>
    Private _FrequencyPointsType As FrequencyPointsType?

    ''' <summary> Gets or sets the cached Frequency Points Type. </summary>
    ''' <value> The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if not set or
    ''' unknown. </value>
    Public Overridable Property FrequencyPointsType As FrequencyPointsType?
        Get
            Return Me._FrequencyPointsType
        End Get
        Protected Set(ByVal value As FrequencyPointsType?)
            If Not Nullable.Equals(Me.FrequencyPointsType, value) Then
                Me._FrequencyPointsType = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Frequency Points Type. </summary>
    ''' <param name="value"> The Frequency Points Type. </param>
    ''' <returns> The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if unknown. </returns>
    Public Function ApplyFrequencyPointsType(ByVal value As FrequencyPointsType) As FrequencyPointsType?
        Me.WriteFrequencyPointsType(value)
        Return Me.QueryFrequencyPointsType()
    End Function

    ''' <summary> Gets or sets the Frequency Points Type query command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:COLL:FPO?" </remarks>
    Protected Overridable ReadOnly Property FrequencyPointsTypeQueryCommand As String

    ''' <summary> Queries the Frequency Points Type. </summary>
    ''' <returns> The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if unknown. </returns>
    Public Function QueryFrequencyPointsType() As FrequencyPointsType?
        If String.IsNullOrWhiteSpace(Me.FrequencyPointsTypeQueryCommand) Then
            Me.FrequencyPointsType = New FrequencyPointsType?
        Else
            Dim mode As String = Me.FrequencyPointsType.ToString
            Me.Session.MakeEmulatedReplyIfEmpty(mode)
            mode = Me.Session.QueryTrimEnd(Me.FrequencyPointsTypeQueryCommand)
            If String.IsNullOrWhiteSpace(mode) Then
                Dim message As String = "Failed fetching Frequency Points Type"
                Debug.Assert(Not Debugger.IsAttached, message)
                Me.FrequencyPointsType = New FrequencyPointsType?
            Else
                Dim se As New StringEnumerator(Of FrequencyPointsType)
                Me.FrequencyPointsType = se.ParseContained(mode.BuildDelimitedValue)
            End If
        End If
        Return Me.FrequencyPointsType
    End Function

    ''' <summary> Gets or sets the Frequency Points Type command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:COLL:FPO {0}" </remarks>
    Protected Overridable ReadOnly Property FrequencyPointsTypeCommandFormat As String

    ''' <summary> Writes the Frequency Points Type without reading back the value from the device. </summary>
    ''' <param name="value"> The Frequency Points Type. </param>
    ''' <returns> The <see cref="FrequencyPointsType">Frequency Points Type</see> or none if unknown. </returns>
    Public Function WriteFrequencyPointsType(ByVal value As FrequencyPointsType) As FrequencyPointsType?
        If Not String.IsNullOrWhiteSpace(Me.FrequencyPointsTypeCommandFormat) Then
            Me.Session.WriteLine(Me.FrequencyPointsTypeCommandFormat, value.ExtractBetween())
        End If
        Me.FrequencyPointsType = value
        Return Me.FrequencyPointsType
    End Function

#End Region

#Region " SWEEP POINTS "

    Private _SweepPoints As Integer?
    ''' <summary> Gets or sets the cached Sweep Points. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Sweep Points or none if not set or unknown. </value>
    Public Overloads Property SweepPoints As Integer?
        Get
            Return Me._SweepPoints
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.SweepPoints, value) Then
                Me._SweepPoints = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Points. </summary>
    ''' <param name="value"> The current Sweep Points. </param>
    ''' <returns> The SweepPoints or none if unknown. </returns>
    Public Function ApplySweepPoints(ByVal value As Integer) As Integer?
        Me.WriteSweepPoints(value)
        Return Me.QuerySweepPoints()
    End Function

    ''' <summary> Gets Sweep Points query command. </summary>
    ''' <value> The Sweep Points query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:SWE:POIN?" </remarks>
    Protected Overridable ReadOnly Property SweepPointsQueryCommand As String

    ''' <summary> Queries the current Sweep Points. </summary>
    ''' <returns> The Sweep Points or none if unknown. </returns>
    Public Function QuerySweepPoints() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.SweepPointsQueryCommand) Then
            Me.SweepPoints = Me.Session.Query(0I, Me.SweepPointsQueryCommand)
        End If
        Return Me.SweepPoints
    End Function

    ''' <summary> Gets Sweep Points command format. </summary>
    ''' <value> The Sweep Points command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:SWE:POIN {0}" </remarks>
    Protected Overridable ReadOnly Property SweepPointsCommandFormat As String

    ''' <summary> Write the Sweep Points without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsSweepPoints. </param>
    ''' <returns> The PointsSweepPoints or none if unknown. </returns>
    Public Function WriteSweepPoints(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.SweepPointsCommandFormat) Then
            Me.Session.WriteLine(Me.SweepPointsCommandFormat, value)
        End If
        Me.SweepPoints = value
        Return Me.SweepPoints
    End Function

#End Region

#Region " SWEEP START "

    Private _SweepStart As Double?
    ''' <summary> Gets or sets the cached Sweep Start. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Sweep Start or none if not set or unknown. </value>
    Public Overloads Property SweepStart As Double?
        Get
            Return Me._SweepStart
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SweepStart, value) Then
                Me._SweepStart = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Start. </summary>
    ''' <param name="value"> The current Sweep Start. </param>
    ''' <returns> The SweepStart or none if unknown. </returns>
    Public Function ApplySweepStart(ByVal value As Double) As Double?
        Me.WriteSweepStart(value)
        Return Me.QuerySweepStart()
    End Function

    ''' <summary> Gets Sweep Start query command. </summary>
    ''' <value> The Sweep Start query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:FREQ:STAR?" </remarks>
    Protected Overridable ReadOnly Property SweepStartQueryCommand As String

    ''' <summary> Queries the current Sweep Start. </summary>
    ''' <returns> The Sweep Start or none if unknown. </returns>
    Public Function QuerySweepStart() As Double?
        If Not String.IsNullOrWhiteSpace(Me.SweepStartQueryCommand) Then
            Me.SweepStart = Me.Session.Query(0I, Me.SweepStartQueryCommand)
        End If
        Return Me.SweepStart
    End Function

    ''' <summary> Gets Sweep Start command format. </summary>
    ''' <value> The Sweep Start command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:FREQ:STAR {0}" </remarks>
    Protected Overridable ReadOnly Property SweepStartCommandFormat As String

    ''' <summary> Write the Sweep Start without reading back the value from the device. </summary>
    ''' <param name="value"> The current StartSweepStart. </param>
    ''' <returns> The StartSweepStart or none if unknown. </returns>
    Public Function WriteSweepStart(ByVal value As Double) As Double?
        If Not String.IsNullOrWhiteSpace(Me.SweepStartCommandFormat) Then
            Me.Session.WriteLine(Me.SweepStartCommandFormat, value)
        End If
        Me.SweepStart = value
        Return Me.SweepStart
    End Function

#End Region

#Region " SWEEP STOP "

    Private _SweepStop As Double?
    ''' <summary> Gets or sets the cached Sweep Stop. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' trigger model. </remarks>
    ''' <value> The Sweep Stop or none if not set or unknown. </value>
    Public Overloads Property SweepStop As Double?
        Get
            Return Me._SweepStop
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.SweepStop, value) Then
                Me._SweepStop = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Stop. </summary>
    ''' <param name="value"> The current Sweep Stop. </param>
    ''' <returns> The SweepStop or none if unknown. </returns>
    Public Function ApplySweepStop(ByVal value As Double) As Double?
        Me.WriteSweepStop(value)
        Return Me.QuerySweepStop()
    End Function

    ''' <summary> Gets Sweep Stop query command. </summary>
    ''' <value> The Sweep Stop query command. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:FREQ:STOP?" </remarks>
    Protected Overridable ReadOnly Property SweepStopQueryCommand As String

    ''' <summary> Queries the current Sweep Stop. </summary>
    ''' <returns> The Sweep Stop or none if unknown. </returns>
    Public Function QuerySweepStop() As Double?
        If Not String.IsNullOrWhiteSpace(Me.SweepStopQueryCommand) Then
            Me.SweepStop = Me.Session.Query(0I, Me.SweepStopQueryCommand)
        End If
        Return Me.SweepStop
    End Function

    ''' <summary> Gets Sweep Stop command format. </summary>
    ''' <value> The Sweep Stop command format. </value>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:FREQ:STOP {0}" </remarks>
    Protected Overridable ReadOnly Property SweepStopCommandFormat As String

    ''' <summary> Write the Sweep Stop without reading back the value from the device. </summary>
    ''' <param name="value"> The current StopSweepStop. </param>
    ''' <returns> The StopSweepStop or none if unknown. </returns>
    Public Function WriteSweepStop(ByVal value As Double) As Double?
        If Not String.IsNullOrWhiteSpace(Me.SweepStopCommandFormat) Then
            Me.Session.WriteLine(Me.SweepStopCommandFormat, value)
        End If
        Me.SweepStop = value
        Return Me.SweepStop
    End Function

#End Region

#Region " SWEEP TYPE "

    Private _SupportedSweepTypes As SweepType
    ''' <summary>
    ''' Gets or sets the supported Sweep Type.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedSweepTypes() As SweepType
        Get
            Return _SupportedSweepTypes
        End Get
        Set(ByVal value As SweepType)
            If Not Me.SupportedSweepTypes.Equals(value) Then
                Me._SupportedSweepTypes = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> The Sweep Type. </summary>
    Private _SweepType As SweepType?

    ''' <summary> Gets or sets the cached Sweep Type. </summary>
    ''' <value> The <see cref="SweepType">Sweep Type</see> or none if not set or
    ''' unknown. </value>
    Public Overridable Property SweepType As SweepType?
        Get
            Return Me._SweepType
        End Get
        Protected Set(ByVal value As SweepType?)
            If Not Nullable.Equals(Me.SweepType, value) Then
                Me._SweepType = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Sweep Type. </summary>
    ''' <param name="value"> The Sweep Type. </param>
    ''' <returns> The <see cref="SweepType">Sweep Type</see> or none if unknown. </returns>
    Public Function ApplySweepType(ByVal value As SweepType) As SweepType?
        Me.WriteSweepType(value)
        Return Me.QuerySweepType()
    End Function

    ''' <summary> Gets or sets the Sweep Type query command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:SWE:TYPE? </remarks>
    Protected Overridable ReadOnly Property SweepTypeQueryCommand As String

    ''' <summary> Queries the Sweep Type. </summary>
    ''' <returns> The <see cref="SweepType">Sweep Type</see> or none if unknown. </returns>
    Public Function QuerySweepType() As SweepType?
        If String.IsNullOrWhiteSpace(Me.SweepTypeQueryCommand) Then
            Me.SweepType = New SweepType?
        Else
            Dim mode As String = Me.SweepType.ToString
            Me.Session.MakeEmulatedReplyIfEmpty(mode)
            mode = Me.Session.QueryTrimEnd(Me.SweepTypeQueryCommand)
            If String.IsNullOrWhiteSpace(mode) Then
                Dim message As String = "Failed fetching Sweep Type"
                Debug.Assert(Not Debugger.IsAttached, message)
                Me.SweepType = New SweepType?
            Else
                Dim se As New StringEnumerator(Of SweepType)
                Me.SweepType = se.ParseContained(mode.BuildDelimitedValue)
            End If
        End If
        Return Me.SweepType
    End Function

    ''' <summary> Gets or sets the Sweep Type command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:SWE:TYPE {0} </remarks>
    Protected Overridable ReadOnly Property SweepTypeCommandFormat As String

    ''' <summary> Writes the Sweep Type without reading back the value from the device. </summary>
    ''' <param name="value"> The Sweep Type. </param>
    ''' <returns> The <see cref="SweepType">Sweep Type</see> or none if unknown. </returns>
    Public Function WriteSweepType(ByVal value As SweepType) As SweepType?
        If Not String.IsNullOrWhiteSpace(Me.SweepTypeCommandFormat) Then
            Me.Session.WriteLine(Me.SweepTypeCommandFormat, value.ExtractBetween())
        End If
        Me.SweepType = value
        Return Me.SweepType
    End Function

#End Region

End Class

''' <summary>
''' Specifies the adapter types.
''' </summary>
Public Enum AdapterType
    <ComponentModel.Description("None (NONE)")> None = 0
    <ComponentModel.Description("4TP 1m (E4M1)")> E4M1
    <ComponentModel.Description("4TP 2m (E4M2)")> E4M2
End Enum

Public Enum SweepType
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Linear (LIN)")> Linear
    <ComponentModel.Description("Logarithmic (LOG)")> Logarithmic
    <ComponentModel.Description("Segment (SEGM)")> Segment
    <ComponentModel.Description("Power (POW)")> Power
    <ComponentModel.Description("Linear Bias (BIAS)")> LinearBias
    <ComponentModel.Description("Logarithmic Bias (LBI)")> LogarithmicBias
End Enum

Public Enum FrequencyPointsType
    <ComponentModel.Description("Fixed (FIX)")> Fixed
    <ComponentModel.Description("User (USER)")> User
End Enum