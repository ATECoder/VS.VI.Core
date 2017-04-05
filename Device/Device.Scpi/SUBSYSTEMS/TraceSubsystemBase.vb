
''' <summary> Defines the contract that must be implemented by a SCPI Trace Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class TraceSubsystemBase
    Inherits VI.TraceSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="TraceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FeedSource = Scpi.FeedSource.None
        Me.FeedControl = Scpi.FeedControl.Never
    End Sub

#End Region

#Region " FEED SOURCE "

    ''' <summary> The Feed Source. </summary>
    Private _FeedSource As FeedSource?

    ''' <summary> Gets or sets the cached source FeedSource. </summary>
    ''' <value> The <see cref="FeedSource">source Feed Source</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FeedSource As FeedSource?
        Get
            Return Me._FeedSource
        End Get
        Protected Set(ByVal value As FeedSource?)
            If Not Me.FeedSource.Equals(value) Then
                Me._FeedSource = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the source Feed Source. </summary>
    ''' <param name="value"> The  Source Feed Source. </param>
    ''' <returns> The <see cref="FeedSource">source Feed Source</see> or none if unknown. </returns>
    Public Function ApplyFeedSource(ByVal value As FeedSource) As FeedSource?
        Me.WriteFeedSource(value)
        Return Me.QueryFeedSource()
    End Function

    ''' <summary> Gets the feed source query command. </summary>
    ''' <value> The write feed source query command. </value>
    ''' <remarks> SCPI: ":TRAC:FEED". </remarks>
    Protected Overridable ReadOnly Property FeedSourceQueryCommand As String

    ''' <summary> Queries the feed Source. </summary>
    ''' <returns> The <see cref="FeedSource">feed Source</see> or none if unknown. </returns>
    Public Function QueryFeedSource() As FeedSource?
        Me.FeedSource = Me.Query(Of FeedSource)(Me.FeedSourceQueryCommand, Me.FeedSource)
        Return Me.FeedSource
    End Function

    ''' <summary> Gets the feed source command format. </summary>
    ''' <value> The write feed source command format. </value>
    ''' <remarks> SCPI: ":TRAC:FEED {0}". </remarks>
    Protected Overridable ReadOnly Property FeedSourceCommandFormat As String

    ''' <summary> Writes the feed Source without reading back the value from the device. </summary>
    ''' <param name="value"> The Feed Source. </param>
    ''' <returns> The <see cref="FeedSource">feed Source</see> or none if unknown. </returns>
    Public Function WriteFeedSource(ByVal value As FeedSource) As FeedSource?
        Me.FeedSource = Me.Write(Of FeedSource)(Me.FeedSourceCommandFormat, value)
        Return Me.FeedSource
    End Function

#End Region

#Region " FEED CONTROL "

    ''' <summary> The Feed Control. </summary>
    Private _FeedControl As FeedControl?

    ''' <summary> Gets or sets the cached Control FeedControl. </summary>
    ''' <value> The <see cref="FeedControl">Control Feed Control</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property FeedControl As FeedControl?
        Get
            Return Me._FeedControl
        End Get
        Protected Set(ByVal value As FeedControl?)
            If Not Me.FeedControl.Equals(value) Then
                Me._FeedControl = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Control Feed Control. </summary>
    ''' <param name="value"> The  Control Feed Control. </param>
    ''' <returns> The <see cref="FeedControl">Control Feed Control</see> or none if unknown. </returns>
    Public Function ApplyFeedControl(ByVal value As FeedControl) As FeedControl?
        Me.WriteFeedControl(value)
        Return Me.QueryFeedControl()
    End Function

    ''' <summary> Gets the feed Control query command. </summary>
    ''' <value> The write feed Control query command. </value>
    ''' <remarks> SCPI: ":TRACE:FEED:CONTROL" </remarks>
    Protected Overridable ReadOnly Property FeedControlQueryCommand As String

    ''' <summary> Queries the feed Control. </summary>
    ''' <returns> The <see cref="FeedControl">feed Control</see> or none if unknown. </returns>
    Public Function QueryFeedControl() As FeedControl?
        Me.FeedControl = Me.Query(Of FeedControl)(Me.FeedControlQueryCommand, Me.FeedControl)
        Return Me.FeedControl
    End Function

    ''' <summary> Gets the feed Control command format. </summary>
    ''' <value> The write feed Control command format. </value>
    ''' <remarks> SCPI: ":TRACE:FEED:CONTROL" </remarks>
    Protected Overridable ReadOnly Property FeedControlCommandFormat As String

    ''' <summary> Writes the feed Control without reading back the value from the device. </summary>
    ''' <param name="value"> The Feed Control. </param>
    ''' <returns> The <see cref="FeedControl">feed Control</see> or none if unknown. </returns>
    Public Function WriteFeedControl(ByVal value As FeedControl) As FeedControl?
        Me.FeedControl = Me.Write(Of FeedControl)(Me.FeedControlCommandFormat, value)
        Return Me.FeedControl
    End Function

#End Region

End Class

''' <summary> Enumerates the trace feed control. </summary>
Public Enum FeedControl
        <ComponentModel.Description("Not Defined ()")> None
        <ComponentModel.Description("Sense (NEXT)")> [Next]
        <ComponentModel.Description("Sense (NEVE)")> [Never]
    End Enum

    ''' <summary> Enumerates the source of readings. </summary>
    Public Enum FeedSource
        <ComponentModel.Description("Not Defined ()")> None
        <ComponentModel.Description("Sense (SENS)")> Sense
        <ComponentModel.Description("Calculate 1 (CALC)")> Calculate1
        <ComponentModel.Description("Calculate 2 (CALC2)")> Calculate2
        <ComponentModel.Description("Current (CURR)")> Current
        <ComponentModel.Description("Voltage (VOLT)")> Voltage
        <ComponentModel.Description("Resistance (RES)")> Resistance
    End Enum
