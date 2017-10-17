''' <summary> An inner error base class. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/24/2015" by="David" revision=""> Created. </history>
Public MustInherit Class NativeErrorBase

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <param name="errorCode"> The error code. </param>
    Protected Sub New(ByVal errorCode As Integer)
        MyBase.New
        Me._ErrorCode = errorCode
        Me._ResourceName = "n/a"
        Me._NodeNumber = New Integer?
        Me._LastMessageSent = ""
        Me._LastAction = ""
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <param name="errorCode">       The error code. </param>
    ''' <param name="resourceName">    The name of the resource. </param>
    ''' <param name="lastMessageSent"> The last message sent. </param>
    ''' <param name="lastAction">      The last visa action. </param>
    Protected Sub New(ByVal errorCode As Integer, ByVal resourceName As String,
                      ByVal lastMessageSent As String, ByVal lastAction As String)
        MyBase.New
        Me._ErrorCode = errorCode
        Me._ResourceName = resourceName
        Me._LastMessageSent = lastMessageSent
        Me._LastAction = lastAction
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <param name="errorCode">       The error code. </param>
    ''' <param name="resourceName">    The name of the resource. </param>
    ''' <param name="nodeNumber">      The node number. </param>
    ''' <param name="lastMessageSent"> The last message sent. </param>
    ''' <param name="lastAction">      The last visa action. </param>
    Protected Sub New(ByVal errorCode As Integer, ByVal resourceName As String,
                      ByVal nodeNumber As Integer, ByVal lastMessageSent As String,
                      ByVal lastAction As String)
        MyBase.New
        Me._ErrorCode = errorCode
        Me._ResourceName = resourceName
        Me._LastMessageSent = lastMessageSent
        Me._NodeNumber = nodeNumber
        Me._LastAction = lastAction
    End Sub

    ''' <summary> Gets or sets the error code. </summary>
    ''' <value> The error code. </value>
    Public ReadOnly Property ErrorCode As Integer

    ''' <summary> Gets or sets the last action. </summary>
    ''' <value> The last action. </value>
    Public Property LastAction As String

    ''' <summary> Gets or sets the last message sent. </summary>
    ''' <value> The last message sent. </value>
    Public ReadOnly Property LastMessageSent As String

    ''' <summary> Gets or sets the name of the resource. </summary>
    ''' <value> The name of the resource. </value>
    Public Property ResourceName As String

    ''' <summary> Gets or sets the node number. </summary>
    ''' <value> The node number. </value>
    Public Property NodeNumber As Integer?

    ''' <summary> Gets or sets the name of the error code. </summary>
    ''' <value> The name of the error code. </value>
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId:="CodeName")>
    Public MustOverride ReadOnly Property ErrorCodeName() As String

    ''' <summary> Gets or sets information describing the error code. </summary>
    ''' <value> Information describing the error code. </value>
    Public MustOverride ReadOnly Property ErrorCodeDescription() As String

#Region " ERROR OR STATUS DETAILS "

    ''' <summary> Adds an exception data. </summary>
    ''' <param name="exception"> The exception receiving the added data.. </param>
    Public Sub AddExceptionData(ByVal exception As System.Exception)
        If exception IsNot Nothing AndAlso Me.ErrorCode <> 0 Then
            Dim count As Integer = exception.Data.Count
            If Me.ErrorCode > 0 Then
                exception.Data.Add($"{count}-Warning", $"0x{Me.ErrorCode:X}")
            Else
                exception.Data.Add($"{count}-Error", $"-0x{-Me.ErrorCode:X}")
            End If
            If Not String.IsNullOrWhiteSpace(Me.ErrorCodeName) Then exception.Data.Add($"{count}-Name", $"{Me.ErrorCodeName}")
            If Not String.Equals(Me.ErrorCodeName, Me.ErrorCodeDescription, StringComparison.OrdinalIgnoreCase) Then
                If Not String.IsNullOrWhiteSpace(Me.ErrorCodeDescription) Then exception.Data.Add($"{count}-Description", $"{Me.ErrorCodeDescription}")
            End If
            If Not String.IsNullOrWhiteSpace(Me.ResourceName) Then exception.Data.Add($"{count}-Resource", $"{Me.ResourceName}")
            If Me.NodeNumber.HasValue Then exception.Data.Add($"{count}-Node", $"{Me.NodeNumber}")
            If Not String.IsNullOrWhiteSpace(Me.LastAction) Then exception.Data.Add($"{count}-LastAction", $"{Me.LastAction}")
            If Not String.IsNullOrWhiteSpace(Me.LastMessageSent) Then exception.Data.Add($"{count}-LastMessageSent", $"{Me.LastMessageSent}")
        End If
    End Sub

    ''' <summary> Builds an error code or status message. </summary>
    ''' <param name="lastAction"> The last visa action. </param>
    Public Function BuildErrorCodeDetails(ByVal lastAction As String) As String
        Return $"{lastAction} {Me.BuildErrorCodeDetails()}."
    End Function

    ''' <summary> Builds an error code or status message. </summary>
    Public Function BuildErrorCodeDetails() As String

        Const innerErrorConstructName As String = "Native I/O"
        If Me.ErrorCode = 0 Then
            Return "OK"
        Else
            Dim visaMessage As New System.Text.StringBuilder
            If Me.ErrorCode > 0 Then
                visaMessage.AppendFormat("{0} Warning {1:X}/{1}", innerErrorConstructName, Me.ErrorCode)
            Else
                visaMessage.AppendFormat("{0} Error {1:X}/{1}", innerErrorConstructName, Me.ErrorCode)
            End If
            Dim name As String = Me.ErrorCodeName()
            Dim description As String = Me.ErrorCodeDescription()
            If String.Equals(name, description, StringComparison.CurrentCultureIgnoreCase) Then
                visaMessage.AppendFormat(Globalization.CultureInfo.CurrentCulture, ": {0}.", name)
            Else
                visaMessage.AppendFormat(Globalization.CultureInfo.CurrentCulture, " {0}: {1}.", name, description)
            End If
            Return visaMessage.ToString
        End If

    End Function

#End Region

End Class
