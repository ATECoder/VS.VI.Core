Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary> Defines the contract that must be implemented by a SCPI Compensation Channel Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class CompensateChannelSubsystemBase
    Inherits VI.CompensateChannelSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class.
    ''' </summary>
    ''' <param name="compensationType"> Type of the compensation. </param>
    ''' <param name="channelNumber">    A reference to a <see cref="StatusSubsystemBase">status
    '''                                 subsystem</see>. </param>
    ''' <param name="statusSubsystem">  The status subsystem. </param>
    Protected Sub New(ByVal compensationType As Scpi.CompensationTypes, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(channelNumber, statusSubsystem)
        Me.ApplyCompensationType(compensationType)
    End Sub

#End Region

#Region " COMPENSATION TYPE "

    Private _SupportedCompensationTypes As CompensationTypes
    ''' <summary>
    ''' Gets or sets the supported Compensation Type.
    ''' This is a subset of the functions supported by the instrument.
    ''' </summary>
    Public Property SupportedCompensationTypes() As CompensationTypes
        Get
            Return _SupportedCompensationTypes
        End Get
        Set(ByVal value As CompensationTypes)
            If Not Me.SupportedCompensationTypes.Equals(value) Then
                Me._SupportedCompensationTypes = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the compensation type code. </summary>
    ''' <value> The compensation type code. </value>
    Protected ReadOnly Property CompensationTypeCode As String

    ''' <summary> The Compensation Type. </summary>
    Private _CompensationType As CompensationTypes?

    Private Sub ApplyCompensationType(ByVal value As CompensationTypes)
        Me._CompensationType = value
        Me._CompensationTypeCode = value.ExtractBetween
    End Sub

    ''' <summary> Gets or sets the cached source CompensationType. </summary>
    ''' <value> The <see cref="CompensationTypes">source Compensation Type</see> or none if not set or
    ''' unknown. </value>
    Public Overridable Property CompensationType As CompensationTypes?
        Get
            Return Me._CompensationType
        End Get
        Protected Set(ByVal value As CompensationTypes?)
            If Not Nullable.Equals(Me.CompensationType, value) Then
                Me._CompensationType = value
                If value.HasValue Then
                    Me.ApplyCompensationType(value.Value)
                Else
                    Me._CompensationTypeCode = ""
                End If
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

End Class

''' <summary> A bit-field of flags for specifying compensation types. </summary>
<Flags>
Public Enum CompensationTypes
    <ComponentModel.Description("None")> None = 0
    <ComponentModel.Description("Open (OPEN)")> OpenCircuit = 1
    <ComponentModel.Description("Short (SHOR)")> ShortCircuit = OpenCircuit << 1
    <ComponentModel.Description("Load (LOAD)")> Load = ShortCircuit << 1
End Enum

