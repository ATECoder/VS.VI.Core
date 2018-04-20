Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary>  Defines the contract that must be implemented by a Sense Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SenseSubsystemBase
    Inherits SourceMeasureUnitBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.SenseMode = SenseActionMode.Local
    End Sub

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

''' <summary> Specifies the sense modes. </summary>
Public Enum SenseActionMode
    <Description("None")> None
    <Description("Remote (SENSE_REMOTE)")> Remote
    <Description("Local (SENSE_LOCAL)")> Local
End Enum

