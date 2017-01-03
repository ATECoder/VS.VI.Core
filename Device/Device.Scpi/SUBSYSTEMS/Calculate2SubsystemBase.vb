Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Calculate 2 Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class Calculate2SubsystemBase
    Inherits VI.Calculate2SubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FeedSource = VI.Scpi.FeedSource.Voltage
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
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FeedSource))
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

    ''' <summary> Gets or sets the feed source query command. </summary>
    ''' <value> The feed source query command. </value>
    ''' <remarks> SCPI: ":CALC2:FEED?" </remarks>
    Protected Overridable ReadOnly Property FeedSourceQueryCommand As String

    ''' <summary> Queries the feed Source. </summary>
    ''' <returns> The <see cref="FeedSource">feed Source</see> or none if unknown. </returns>
    Public Function QueryFeedSource() As FeedSource?
        Dim currentValue As String = Me.FeedSource.ToString
        If String.IsNullOrEmpty(Me.Session.EmulatedReply) Then Me.Session.EmulatedReply = currentValue
        currentValue = Me.Session.QueryTrimEnd(Me.FeedSourceQueryCommand)
        If String.IsNullOrWhiteSpace(currentValue) Then
            Me.FeedSource = New FeedSource?
        Else
            Dim se As New StringEnumerator(Of FeedSource)
            Me.FeedSource = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.FeedSource
    End Function

    ''' <summary> Gets or sets the feed source command format. </summary>
    ''' <value> The write feed source command format. </value>
    ''' <remarks> SCPI: "CALC2:FEED {0}". </remarks>
    Protected Overridable ReadOnly Property FeedSourceCommandFormat As String

    ''' <summary> Writes the feed Source without reading back the value from the device. </summary>
    ''' <param name="value"> The Feed Source. </param>
    ''' <returns> The <see cref="FeedSource">feed Source</see> or none if unknown. </returns>
    Public Function WriteFeedSource(ByVal value As FeedSource) As FeedSource?
        If Not String.IsNullOrWhiteSpace(Me.FeedSourceCommandFormat) Then
            Me.Session.WriteLine(Me.FeedSourceCommandFormat, value.ExtractBetween())
        End If
        Me.FeedSource = value
        Return Me.FeedSource
    End Function

#End Region

#Region " Limit1 1 "

#Region " Limit1 ENABLED "

    ''' <summary> Limit1 enabled. </summary>
    Private _Limit1Enabled As Boolean?

    ''' <summary> Gets or sets the cached Limit1 Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Limit1 Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Limit1Enabled As Boolean?
        Get
            Return Me._Limit1Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit1Enabled, value) Then
                Me._Limit1Enabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Limit1Enabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit1Enabled(ByVal value As Boolean) As Boolean?
        Me.WriteLimit1Enabled(value)
        Return Me.QueryLimit1Enabled()
    End Function

    ''' <summary> Gets or sets the Limit1 enabled query command. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    ''' <remarks> SCPI: "CURR:AVER:STAT?" </remarks>
    Protected Overridable ReadOnly Property Limit1EnabledQueryCommand As String

    ''' <summary> Queries the Limit1 Enabled sentinel. Also sets the
    ''' <see cref="Limit1Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimit1Enabled() As Boolean?
        Me.Limit1Enabled = Me.Query(Me.Limit1Enabled, Me.Limit1EnabledQueryCommand)
        Return Me.Limit1Enabled
    End Function

    ''' <summary> Gets or sets the Limit1 enabled command Format. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    ''' <remarks> SCPI: "CURR:AVER:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property Limit1EnabledCommandFormat As String

    ''' <summary> Writes the Limit1 Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteLimit1Enabled(ByVal value As Boolean) As Boolean?
        Me.Limit1Enabled = Me.Write(value, Me.Limit1EnabledCommandFormat)
        Return Me.Limit1Enabled
    End Function

#End Region

#Region " Limit1 LOWER LEVEL "

    ''' <summary> The Limit1 Lower Level. </summary>
    Private _Limit1LowerLevel As Double?

    ''' <summary> Gets or sets the cached Limit1 Lower Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit1LowerLevel As Double?
        Get
            Return Me._Limit1LowerLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit1LowerLevel, value) Then
                Me._Limit1LowerLevel = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Limit1LowerLevel))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Lower Level. </summary>
    ''' <param name="value"> The Limit1 Lower Level. </param>
    ''' <returns> The Limit1 Lower Level. </returns>
    Public Function ApplyLimit1LowerLevel(ByVal value As Double) As Double?
        Me.WriteLimit1LowerLevel(value)
        Return Me.QueryLimit1LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Lower Level query command. </summary>
    ''' <value> The Limit1 Lower Level query command. </value>
    Protected Overridable ReadOnly Property Limit1LowerLevelQueryCommand As String

    ''' <summary> Queries The Limit1 Lower Level. </summary>
    ''' <returns> The Limit1 Lower Level or none if unknown. </returns>
    Public Function QueryLimit1LowerLevel() As Double?
        Me.Limit1LowerLevel = Me.Query(Me.Limit1LowerLevel, Me.Limit1LowerLevelQueryCommand)
        Return Me.Limit1LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Lower Level command format. </summary>
    ''' <value> The Limit1 Lower Level command format. </value>
    Protected Overridable ReadOnly Property Limit1LowerLevelCommandFormat As String

    ''' <summary> Writes The Limit1 Lower Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit1 Lower Level. </remarks>
    ''' <param name="value"> The Limit1 Lower Level. </param>
    ''' <returns> The Limit1 Lower Level. </returns>
    Public Function WriteLimit1LowerLevel(ByVal value As Double) As Double?
        Me.Limit1LowerLevel = Me.Write(value, Me.Limit1LowerLevelCommandFormat)
        Return Me.Limit1LowerLevel
    End Function

#End Region

#Region " Limit1 UPPER LEVEL "

    ''' <summary> The Limit1 Upper Level. </summary>
    Private _Limit1UpperLevel As Double?

    ''' <summary> Gets or sets the cached Limit1 Upper Level. Set to
    ''' <see cref="Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit1UpperLevel As Double?
        Get
            Return Me._Limit1UpperLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit1UpperLevel, value) Then
                Me._Limit1UpperLevel = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Limit1UpperLevel))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Upper Level. </summary>
    ''' <param name="value"> The Limit1 Upper Level. </param>
    ''' <returns> The Limit1 Upper Level. </returns>
    Public Function ApplyLimit1UpperLevel(ByVal value As Double) As Double?
        Me.WriteLimit1UpperLevel(value)
        Return Me.QueryLimit1UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Upper Level query command. </summary>
    ''' <value> The Limit1 Upper Level query command. </value>
    Protected Overridable ReadOnly Property Limit1UpperLevelQueryCommand As String

    ''' <summary> Queries The Limit1 Upper Level. </summary>
    ''' <returns> The Limit1 Upper Level or none if unknown. </returns>
    Public Function QueryLimit1UpperLevel() As Double?
        Me.Limit1UpperLevel = Me.Query(Me.Limit1UpperLevel, Me.Limit1UpperLevelQueryCommand)
        Return Me.Limit1UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Upper Level command format. </summary>
    ''' <value> The Limit1 Upper Level command format. </value>
    Protected Overridable ReadOnly Property Limit1UpperLevelCommandFormat As String

    ''' <summary> Writes The Limit1 Upper Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit1 Upper Level. </remarks>
    ''' <param name="value"> The Limit1 Upper Level. </param>
    ''' <returns> The Limit1 Upper Level. </returns>
    Public Function WriteLimit1UpperLevel(ByVal value As Double) As Double?
        Me.Limit1UpperLevel = Me.Write(value, Me.Limit1UpperLevelCommandFormat)
        Return Me.Limit1UpperLevel
    End Function

#End Region


#End Region

End Class
