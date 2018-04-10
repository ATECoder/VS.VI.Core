Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Imports isr.VI.ComboBoxExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Sense channel Subsystem. </summary>
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
    Inherits VI.SenseChannelSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    ''' <param name="channelNumber">   A reference to a <see cref="StatusSubsystemBase">status
    '''                                subsystem</see>. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(channelNumber, statusSubsystem)
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
