Imports isr.VI.ExceptionExtensions
''' <summary> Defines the contract that must be implemented by an Output Subsystem. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="12/12/2013" by="David" revision="3.0.5093"> Created. </history>
Public MustInherit Class AccessSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="OutputSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.CertifyTimeout = TimeSpan.FromMilliseconds(20000)
        Me.Certified = New Boolean?
    End Sub

#End Region

#Region " CERTIFY "

    Private _CertifyTimeout As TimeSpan

    ''' <summary> Gets or sets the timeout time span allowed for certifying the device. </summary>
    Public Property CertifyTimeout As TimeSpan
        Get
            Return Me._CertifyTimeout
        End Get
        Set(ByVal value As TimeSpan)
            If Not Me.CertifyTimeout.Equals(value) Then
                Me._CertifyTimeout = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> State of the output on. </summary>
    Private _Certified As Boolean?

    ''' <summary> Gets or sets the cached certification sentinel. </summary>
    ''' <value> <c>null</c> if not known; <c>True</c> if certified on; otherwise, <c>False</c>. </value>
    Public Property Certified As Boolean?
        Get
            Return Me._Certified
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Certified, value) Then
                Me._Certified = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Certifies. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>null</c> if not known; <c>True</c> if certified on; otherwise, <c>False</c>. </returns>
    Public MustOverride Function Certify(ByVal value As String) As Boolean?

    ''' <summary> Tries to certify. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>True</c> if certified on; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryCertify(ByVal value As String) As Boolean
        Try
            Return Me.Certify(value).GetValueOrDefault(False)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                                      $"{ex.Message} occurred certifying access;. {ex.ToFullBlownString}")
            Return False
        End Try
    End Function

#End Region

#Region " RESOURCES "

    Private _CertifiedInstruments As String
    ''' <summary> Gets the list of certified instruments. </summary>
    ''' <value> The certified instruments. </value>
    Public Property CertifiedInstruments() As String
        Get
            Return Me._certifiedInstruments
        End Get
        Protected Set(ByVal value As String)
            Me._certifiedInstruments = value
            Me.SafePostPropertyChanged()
        End Set

    End Property

    Private _ReleasedInstruments As String

    ''' <summary> Gets the list of released instruments. </summary>
    ''' <value> The released instruments. </value>
    Public Property ReleasedInstruments() As String
        Get
            Return Me._releasedInstruments
        End Get
        Set(ByVal value As String)
            Me._releasedInstruments = value
            Me.SafePostPropertyChanged()
        End Set
    End Property

#End Region

#Region " ACCESS MANAGEMENT "

    ''' <summary> Checks if the custom scripts loaded successfully. </summary>
    ''' <returns> <c>True</c> if loaded; otherwise, <c>False</c>. </returns>
    Public Overridable Function Loaded() As Boolean
        Return False
    End Function

    ''' <summary> Gets the release code for the controller instrument. </summary>
    ''' <returns> The release value of the controller instrument. </returns>
    Public Overridable Function ReleaseValue(ByVal serialNumber As String) As String
        Return ""
    End Function

#End Region

End Class
