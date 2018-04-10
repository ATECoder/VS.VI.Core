Imports System.Windows.Forms
Imports System.ComponentModel
Partial Public Class SessionBase

    ''' <summary> Initializes the service request register bits. </summary>
    Private Sub _InitializeServiceRequestRegisterBits()
        Me._MessageAvailableBits = VI.Pith.ServiceRequests.MessageAvailable
        Me._ErrorAvailableBits = VI.Pith.ServiceRequests.ErrorAvailable
        Me._MeasurementAvailableBits = VI.Pith.ServiceRequests.MeasurementEvent
        Me._OperationAvailableBits = VI.Pith.ServiceRequests.OperationEvent
        Me._QuestionableAvailableBits = VI.Pith.ServiceRequests.QuestionableEvent
        Me._StandardEventAvailableBits = VI.Pith.ServiceRequests.StandardEvent
    End Sub

#Region " SERVICE REQUEST REGISTER EVENTS: ERROR "

    Private _ErrorAvailableBits As VI.Pith.ServiceRequests
    ''' <summary> Gets or sets bits that would be set for detecting if an error is available. </summary>
    ''' <value> The error available bits. </value>
    Public Property ErrorAvailableBits() As VI.Pith.ServiceRequests
        Get
            Return Me._ErrorAvailableBits
        End Get
        Set(ByVal value As VI.Pith.ServiceRequests)
            If Not value.Equals(Me.ErrorAvailableBits) Then
                Me._ErrorAvailableBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _ErrorAvailable As Boolean
    ''' <summary> Gets or sets a value indicating whether [Error available]. </summary>
    ''' <value> <c>True</c> if [Error available]; otherwise, <c>False</c>. </value>
    Public Property ErrorAvailable As Boolean
        Get
            Return Me._ErrorAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            If value OrElse Not value.Equals(Me.ErrorAvailable) Then
                Me._ErrorAvailable = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " SERVICE REQUEST REGISTER EVENTS: MESSAGE AVAILABLE "

    ''' <summary> Checks if message is available. </summary>
    ''' <returns> <c>True</c> if the message available bit is on; otherwise, <c>False</c>. </returns>
    Public Function IsMessageAvailable() As Boolean
        Return Me.IsMessageAvailable(VI.Pith.ServiceRequests.MessageAvailable)
    End Function

    ''' <summary> Checks if message is available. </summary>
    ''' <param name="pollInterval"> The poll interval. </param>
    ''' <param name="timeout">      Specifies the time to wait for message available. </param>
    ''' <returns> <c>True</c> if the message available bit is on; otherwise, <c>False</c>. </returns>
    Public Function IsMessageAvailable(ByVal pollInterval As TimeSpan, ByVal timeout As TimeSpan) As Boolean
        Return Me.IsMessageAvailable(VI.Pith.ServiceRequests.MessageAvailable, pollInterval, timeout)
    End Function

    ''' <summary> Checks if message is available. </summary>
    ''' <param name="messageAvailableBits"> The message available bits. </param>
    ''' <param name="pollInterval">         The poll interval. </param>
    ''' <param name="timeout">              Specifies the time to wait for message available. </param>
    ''' <returns> <c>True</c> if the message available bit is on; otherwise, <c>False</c>. </returns>
    Public Function IsMessageAvailable(ByVal messageAvailableBits As Integer, ByVal pollInterval As TimeSpan, ByVal timeout As TimeSpan) As Boolean
        Dim endTime As DateTime = DateTime.UtcNow.Add(timeout)
        Dim messageAvailable As Boolean = Me.IsMessageAvailable(messageAvailableBits)
        Do Until endTime < DateTime.UtcNow OrElse messageAvailable
            Threading.Thread.Sleep(pollInterval)
            Application.DoEvents()
            messageAvailable = Me.IsMessageAvailable(messageAvailableBits)
        Loop
        Return messageAvailable
    End Function

#End Region

#Region " SERVICE REQUEST REGISTER EVENTS: MESSAGE "

    Private _MessageAvailableBits As VI.Pith.ServiceRequests
    ''' <summary> Gets or sets bits that would be set for detecting if an Message is available. </summary>
    ''' <value> The Message available bits. </value>
    Public Property MessageAvailableBits() As VI.Pith.ServiceRequests
        Get
            Return Me._MessageAvailableBits
        End Get
        Set(ByVal value As VI.Pith.ServiceRequests)
            If Not value.Equals(Me.MessageAvailableBits) Then
                Me._MessageAvailableBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _MessageAvailable As Boolean
    ''' <summary> Gets or sets a value indicating whether [Message available]. </summary>
    ''' <value> <c>True</c> if [Message available]; otherwise, <c>False</c>. </value>
    Public Property MessageAvailable As Boolean
        Get
            Return Me._MessageAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            If value OrElse Not value.Equals(Me.MessageAvailable) Then
                Me._MessageAvailable = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Checks if message is available. </summary>
    ''' <param name="messageAvailableBits"> The message available bits. </param>
    ''' <returns> <c>True</c> if the message available bit is on; otherwise, <c>False</c>. </returns>
    Public Function IsMessageAvailable(ByVal messageAvailableBits As Integer) As Boolean
        Me.ReadServiceRequestStatus()
        If Not Me.MessageAvailableBits.Equals(messageAvailableBits) Then
            Me.MessageAvailable = (Me.ServiceRequestStatus And messageAvailableBits) <> 0
        End If
        Return Me.MessageAvailable
    End Function

    ''' <summary> Checks if message is available. </summary>
    ''' <remarks> Delays looking for the message status by the status latency to make sure the
    ''' instrument had enough time to process the previous command. </remarks>
    ''' <param name="latency">     The latency. </param>
    ''' <param name="repeatCount"> Number of repeats. </param>
    ''' <returns> <c>True</c> if message is available. </returns>
    Public Function IsMessageAvailable(ByVal latency As TimeSpan, ByVal repeatCount As Integer) As Boolean
        Do
            If latency > TimeSpan.Zero Then
                Threading.Thread.Sleep(latency)
            End If
            Me.ReadServiceRequestStatus()
            repeatCount -= 1
        Loop Until repeatCount <= 0 OrElse Me.MessageAvailable
        Return Me.MessageAvailable
    End Function

#End Region

#Region " SERVICE REQUEST REGISTER EVENTS: MEASUREMENT "

    Private _MeasurementAvailableBits As VI.Pith.ServiceRequests
    ''' <summary> Gets or sets bits that would be set for detecting if an Measurement is available. </summary>
    ''' <value> The Measurement available bits. </value>
    Public Property MeasurementAvailableBits() As VI.Pith.ServiceRequests
        Get
            Return Me._MeasurementAvailableBits
        End Get
        Set(ByVal value As VI.Pith.ServiceRequests)
            If Not value.Equals(Me.MeasurementAvailableBits) Then
                Me._MeasurementAvailableBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _MeasurementAvailable As Boolean
    ''' <summary> Gets or sets a value indicating whether [Measurement available]. </summary>
    ''' <value> <c>True</c> if [Measurement available]; otherwise, <c>False</c>. </value>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            If value OrElse Not value.Equals(Me.MeasurementAvailable) Then
                Me._MeasurementAvailable = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " SERVICE REQUEST REGISTER EVENTS: OPERATION "

    Private _OperationAvailableBits As VI.Pith.ServiceRequests
    ''' <summary> Gets or sets bits that would be set for detecting if an Operation is available. </summary>
    ''' <value> The Operation available bits. </value>
    Public Property OperationAvailableBits() As VI.Pith.ServiceRequests
        Get
            Return Me._OperationAvailableBits
        End Get
        Set(ByVal value As VI.Pith.ServiceRequests)
            If Not value.Equals(Me.OperationAvailableBits) Then
                Me._OperationAvailableBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _OperationAvailable As Boolean
    ''' <summary> Gets or sets a value indicating whether [Operation available]. </summary>
    ''' <value> <c>True</c> if [Operation available]; otherwise, <c>False</c>. </value>
    Public Property OperationAvailable As Boolean
        Get
            Return Me._OperationAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            If value OrElse Not value.Equals(Me.OperationAvailable) Then
                Me._OperationAvailable = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " SERVICE REQUEST REGISTER EVENTS: QUESTIONABLE "

    Private _QuestionableAvailableBits As VI.Pith.ServiceRequests
    ''' <summary> Gets or sets bits that would be set for detecting if an Questionable is available. </summary>
    ''' <value> The Questionable available bits. </value>
    Public Property QuestionableAvailableBits() As VI.Pith.ServiceRequests
        Get
            Return Me._QuestionableAvailableBits
        End Get
        Set(ByVal value As VI.Pith.ServiceRequests)
            If Not value.Equals(Me.QuestionableAvailableBits) Then
                Me._QuestionableAvailableBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _QuestionableAvailable As Boolean
    ''' <summary> Gets or sets a value indicating whether [Questionable available]. </summary>
    ''' <value> <c>True</c> if [Questionable available]; otherwise, <c>False</c>. </value>
    Public Property QuestionableAvailable As Boolean
        Get
            Return Me._QuestionableAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            If value OrElse Not value.Equals(Me.QuestionableAvailable) Then
                Me._QuestionableAvailable = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " SERVICE REQUEST REGISTER EVENTS: STANDARD EVENT "

    Private _StandardEventAvailableBits As VI.Pith.ServiceRequests
    ''' <summary> Gets or sets bits that would be set for detecting if an Standard Event is available. </summary>
    ''' <value> The Standard Event available bits. </value>
    Public Property StandardEventAvailableBits() As VI.Pith.ServiceRequests
        Get
            Return Me._StandardEventAvailableBits
        End Get
        Set(ByVal value As VI.Pith.ServiceRequests)
            If Not value.Equals(Me.StandardEventAvailableBits) Then
                Me._StandardEventAvailableBits = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _StandardEventAvailable As Boolean
    ''' <summary> Gets or sets a value indicating whether [StandardEvent available]. </summary>
    ''' <value> <c>True</c> if [StandardEvent available]; otherwise, <c>False</c>. </value>
    Public Property StandardEventAvailable As Boolean
        Get
            Return Me._StandardEventAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            If value OrElse Not value.Equals(Me.StandardEventAvailable) Then
                Me._StandardEventAvailable = value
                Me.SafeSendPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " SERVICE REQUEST "

    ''' <summary> Gets or sets the standard service enable command format. </summary>
    ''' <value> The standard service enable command format. </value>
    ''' <remarks> <see cref="VI.Pith.Ieee488.Syntax.StandardServiceEnableCommandFormat"></see></remarks>
    Public Property StandardServiceEnableCommandFormat As String

    ''' <summary> Program the device to issue an SRQ upon any of the SCPI events. Uses *ESE to select
    ''' (mask) the events that will issue SRQ and  *SRE to select (mask) the event registers to be
    ''' included in the bits that will issue an SRQ. </summary>
    ''' <param name="standardEventEnableBitmask">  Specifies standard events will issue an SRQ. </param>
    ''' <param name="serviceRequestEnableBitmask"> Specifies which status registers will issue an
    ''' SRQ. </param>
    Public Sub EnableServiceRequest(ByVal standardEventEnableBitmask As VI.Pith.StandardEvents,
                                    ByVal serviceRequestEnableBitmask As VI.Pith.ServiceRequests)
        Me.ReadServiceRequestStatus()
        If Not String.IsNullOrWhiteSpace(Me.StandardServiceEnableCommandFormat) Then
            Me.WriteLine(Me.StandardServiceEnableCommandFormat, CInt(standardEventEnableBitmask), CInt(serviceRequestEnableBitmask))
        End If
    End Sub

    ''' <summary> Enabled detection of completion. </summary>
    ''' <remarks> 3475. Add Or VI.Pith.Ieee4882.ServiceRequests.OperationEvent. </remarks>
    Public Sub EnableWaitComplete()
        Me.EnableServiceRequest(StandardEvents.All And Not VI.Pith.StandardEvents.RequestControl, VI.Pith.ServiceRequests.StandardEvent)
    End Sub

    ''' <summary> The service request status. </summary>
    Private _ServiceRequestStatus As VI.Pith.ServiceRequests

    ''' <summary> Gets or sets the cached service request Status. </summary>
    ''' <remarks> The service request status is posted to be parsed by the status subsystem that is
    ''' specific to the instrument at hand. This is critical to the proper workings of the status
    ''' subsystem. The service request status is posted asynchronously. This may not be processed
    ''' fast enough to determine the next action. Not sure how to address this at this time. </remarks>
    ''' <value> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    ''' Requests</see>. </value>
    Public Property ServiceRequestStatus() As VI.Pith.ServiceRequests
        Get
            Return Me._ServiceRequestStatus
        End Get
        Set(ByVal value As VI.Pith.ServiceRequests)
            Dim isNewValue As Boolean = value <> VI.Pith.ServiceRequests.None OrElse Not value.Equals(Me.ServiceRequestStatus)
            Me._ServiceRequestStatus = value
            Me.ErrorAvailable = (value And Me.ErrorAvailableBits) <> 0
            Me.MessageAvailable = (value And Me.MessageAvailableBits) <> 0
            Me.MeasurementAvailable = (value And Me.MeasurementAvailableBits) <> 0
            Me.OperationAvailable = (value And Me.OperationAvailableBits) <> 0
            Me.QuestionableAvailable = (value And Me.QuestionableAvailableBits) <> 0
            Me.StandardEventAvailable = (value And Me.StandardEventAvailableBits) <> 0
            If isNewValue Then Me.SafeSendPropertyChanged()
        End Set
    End Property

    ''' <summary> Reads the service request Status. </summary>
    ''' <returns> <c>null</c> if value is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    ''' Requests</see>. </returns>
    Public Function ReadServiceRequestStatus() As VI.Pith.ServiceRequests
        Me.ServiceRequestStatus = Me.ReadStatusByte()
        Return Me.ServiceRequestStatus
    End Function

#End Region

End Class
