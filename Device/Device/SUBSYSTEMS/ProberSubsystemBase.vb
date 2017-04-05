Imports isr.Core.Pith.EscapeSequencesExtensions
''' <summary> Defines the contract that must be implemented by a Prober Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class ProberSubsystemBase
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
        Me.ErrorRead = False
        Me.LastReading = ""
        Me.IdentityRead = False
        Me.MessageCompleted = False
        Me.MessageFailed = False
        Me.PatternCompleteReceived = New Boolean?
        Me.SetModeSent = False
        Me.IsFirstTestStart = New Boolean?
        Me.RetestRequested = New Boolean?
        Me.TestAgainRequested = New Boolean?
        Me.TestCompleteSent = New Boolean?
        Me.TestStartReceived = New Boolean?
        Me.WaferStartReceived = New Boolean?
    End Sub

#End Region

#Region " ERROR READ "

    Private _ErrorRead As Boolean

    ''' <summary> Gets or sets the Error Read sentinel. </summary>
    ''' <value> The Error Read. </value>
    Public Property ErrorRead As Boolean
        Get
            Return Me._ErrorRead
        End Get
        Set(ByVal value As Boolean)
            Me._ErrorRead = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " IDENTITY READ "

    Private _IdentityRead As Boolean

    ''' <summary> Gets or sets the Identity Read sentinel. </summary>
    ''' <value> The Identity Read. </value>
    Public Property IdentityRead As Boolean
        Get
            Return Me._IdentityRead
        End Get
        Set(ByVal value As Boolean)
            Me._IdentityRead = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " MESSAGE FAILED "

    Private _MessageFailed As Boolean

    ''' <summary> Gets or sets the message failed. </summary>
    ''' <value> The message failed. </value>
    Public Property MessageFailed As Boolean
        Get
            Return Me._MessageFailed
        End Get
        Set(ByVal value As Boolean)
            Me._MessageFailed = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " MESSAGE COMPLETED "

    Private _MessageCompleted As Boolean

    ''' <summary> Gets or sets the message Completed. </summary>
    ''' <value> The message Completed. </value>
    Public Property MessageCompleted As Boolean
        Get
            Return Me._MessageCompleted
        End Get
        Set(ByVal value As Boolean)
            Me._MessageCompleted = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " PATTERN COMPLETE "

    ''' <summary> A sentinel indicating that the Pattern Complete message was received. </summary>
    Private _PatternCompleteReceived As Boolean?

    ''' <summary> Gets or sets the cached Pattern Complete message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if Pattern Complete was received; otherwise,
    ''' <c>False</c>. </value>
    Public Property PatternCompleteReceived As Boolean?
        Get
            Return Me._PatternCompleteReceived
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._PatternCompleteReceived = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
            If value.GetValueOrDefault(False) Then
                ' if pattern completed, turn off all other flags.
                Me.UnhandledMessageReceived = False
                Me.TestCompleteSent = False
                Me.TestStartReceived = False
                Me.WaferStartReceived = False
            End If
        End Set
    End Property

#End Region

#Region " SET MODE SENT "

    ''' <summary> A sentinel indicating that the Set Mode message was Sent. </summary>
    Private _SetModeSent As Boolean

    ''' <summary> Gets or sets the cached Set Mode message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if Set Mode was Sent; otherwise,
    ''' <c>False</c>. </value>
    Public Property SetModeSent As Boolean
        Get
            Return Me._SetModeSent
        End Get
        Protected Set(ByVal value As Boolean)
            Me._SetModeSent = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " TEST COMPLETE SENT "

    ''' <summary> A sentinel indicating that the Test Complete message was Sent. </summary>
    Private _TestCompleteSent As Boolean?

    ''' <summary> Gets or sets the cached Test Complete message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if Test Complete was Sent; otherwise,
    ''' <c>False</c>. </value>
    Public Property TestCompleteSent As Boolean?
        Get
            Return Me._TestCompleteSent
        End Get
        Set(ByVal value As Boolean?)
            Me._TestCompleteSent = value
            If value.GetValueOrDefault(False) Then
                ' if pattern completed, turn off all other flags.
                Me.UnhandledMessageReceived = False
                Me.TestStartReceived = False
            End If
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " TEST START "

    ''' <summary> A sentinel indicating that the test start message was received. </summary>
    Private _TestStartReceived As Boolean?

    ''' <summary> Gets or sets the cached test start message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if test start was received; otherwise,
    ''' <c>False</c>. </value>
    Public Property TestStartReceived As Boolean?
        Get
            Return Me._TestStartReceived
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._TestStartReceived = value
            If value.GetValueOrDefault(False) Then
                ' turn off relevant sentinels.
                Me.UnhandledMessageReceived = False
                Me.PatternCompleteReceived = False
                Me.TestCompleteSent = False
            End If
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

    ''' <summary> A sentinel indicating that the retest is requested. </summary>
    Private _RetestRequested As Boolean?

    ''' <summary> Gets or sets the cached retest requested sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if retest is request; otherwise,
    ''' <c>False</c>. </value>
    Public Property RetestRequested As Boolean?
        Get
            Return Me._RetestRequested
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._RetestRequested = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

    ''' <summary> A sentinel indicating that the Test Again is requested. </summary>
    Private _TestAgainRequested As Boolean?

    ''' <summary> Gets or sets the cached Test Again requested sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if TestAgain is request; otherwise,
    ''' <c>False</c>. </value>
    Public Property TestAgainRequested As Boolean?
        Get
            Return Me._TestAgainRequested
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._TestAgainRequested = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " FIRST TEST START "

    ''' <summary> A sentinel indicating that the test start message was received. </summary>
    Private _IsFirstTestStart As Boolean?

    ''' <summary> Gets or sets the cached first test start message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if test start was received; otherwise,
    ''' <c>False</c>. </value>
    Public Property IsFirstTestStart As Boolean?
        Get
            Return Me._IsFirstTestStart
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._IsFirstTestStart = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " WAFER START "

    ''' <summary> A sentinel indicating that the Wafer start message was received. </summary>
    Private _WaferStartReceived As Boolean?

    ''' <summary> Gets or sets the cached Wafer start message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if Wafer start was received; otherwise,
    ''' <c>False</c>. </value>
    Public Property WaferStartReceived As Boolean?
        Get
            Return Me._WaferStartReceived
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._WaferStartReceived = value
            If value.GetValueOrDefault(False) Then
                ' turn off relevant sentinels.
                Me.UnhandledMessageReceived = False
                Me.PatternCompleteReceived = False
                Me.TestCompleteSent = False
                Me.TestStartReceived = False
            End If
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " UNHANDLED MESSAGE RECEIVED "

    ''' <summary> A sentinel indicating that the Unhandled Message was received. </summary>
    Private _UnhandledMessageReceived As Boolean?

    ''' <summary> Gets or sets the cached Unhandled Message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if Unhandled Message was received; otherwise,
    ''' <c>False</c>. </value>
    Public Property UnhandledMessageReceived As Boolean?
        Get
            Return Me._UnhandledMessageReceived
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._UnhandledMessageReceived = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " UNHANDLED MESSAGE SENT "

    ''' <summary> A sentinel indicating that the Unhandled Message was Sent. </summary>
    Private _UnhandledMessageSent As Boolean?

    ''' <summary> Gets or sets the cached Unhandled Message sentinel. </summary>
    ''' <value> <c>null</c> if state is not known; <c>True</c> if Unhandled Message was Sent; otherwise,
    ''' <c>False</c>. </value>
    Public Property UnhandledMessageSent As Boolean?
        Get
            Return Me._UnhandledMessageSent
        End Get
        Protected Set(ByVal value As Boolean?)
            Me._UnhandledMessageSent = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

#End Region

#Region " FETCH "

    Private _LastReading As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastReading As String
        Get
            Return Me._LastReading
        End Get
        Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(Me.LastReading) Then
                If String.IsNullOrWhiteSpace(value) Then
                    Return
                Else
                    Me._LastReading = ""
                End If
            End If
            If String.IsNullOrWhiteSpace(value) Then value = ""
            Me._LastReading = value
            Me.SafePostPropertyChanged()
            Windows.Forms.Application.DoEvents()
        End Set
    End Property

    ''' <summary> Parses the message. </summary>
    Public MustOverride Sub ParseReading(ByVal reading As String)

    ''' <summary> Fetches and parses a message from the instrument. The message must already be present. </summary>
    Public Sub FetchAndParse()
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Fetching;. ")
        Me.LastReading = Me.Session.ReadLineTrimEnd
        Me.Talker.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Parsing;. {0}", Me.LastReading)
        Me.ParseReading(Me.LastReading)
    End Sub

#End Region

End Class

