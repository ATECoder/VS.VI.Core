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
    ''' <remarks> SCPI: ":TRAC:FEED?" </remarks>
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
    ''' <remarks> SCPI: ":TRAC:FEED {0}". </remarks>
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

End Class
