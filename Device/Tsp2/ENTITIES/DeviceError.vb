''' <summary> Encapsulates handling a TSP device error. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/8/2013" by="David" revision=""> Created. </history>
Public Class DeviceError
    Inherits VI.DeviceError

#Region " CONSTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DeviceError" /> class 
    '''           specifying no error. </summary>
    Public Sub New()
        MyBase.New(TspSyntax.EventLog.NoErrorCompoundMessage)
    End Sub

    ''' <summary> Initializes a new instance of the <see cref="DeviceError" /> class. </summary>
    ''' <param name="value"> The value. </param>
    Public Sub New(ByVal value As DeviceError)
        MyBase.New(value)
        If value Is Nothing Then
            Me.CompoundErrorMessage = TspSyntax.EventLog.NoErrorCompoundMessage
            Me.ErrorMessage = TspSyntax.EventLog.NoErrorMessage
            Me._ErrorLevel = TspErrorLevel.None
            Me._NodeNumber = 0
        Else
            Me._ErrorLevel = value.ErrorLevel
            Me._NodeNumber = value.NodeNumber
        End If
    End Sub

#End Region

#Region " PARSE "

    ''' <summary> Initializes a new instance of the <see cref="DeviceError" /> class. </summary>
    ''' <param name="compoundError"> The compound error. </param>
    ''' <remarks>Error messages are formatted as follows:<para>
    ''' error#,message,level#,node#.</para>
    ''' <list type="bullet">
    '''   <listheader><description>Error levels are:</description>
    '''   </listheader>
    ''' <item><description>0 - Informational.</description></item>
    ''' <item><description>10 - Informational.</description></item>
    ''' <item><description>30 - Serious.</description></item>
    ''' <item><description>40 - Critical.</description></item>
    ''' </list>
    ''' </remarks>
    Public Overrides Sub Parse(ByVal compoundError As String)
        MyBase.Parse(compoundError)
        If String.IsNullOrWhiteSpace(compoundError) Then
            Me._ErrorLevel = TspErrorLevel.None
            Me._NodeNumber = 0
        Else
            Dim parts() As String = compoundError.Split(","c)
            Dim value As Integer = 0
            If parts.Length > 2 Then
                If Integer.TryParse(parts(2), value) Then
                    Me._ErrorLevel = CType(value, TspErrorLevel)
                End If
            End If
            If parts.Length > 3 Then
                If Integer.TryParse(parts(3), value) Then
                    Me._NodeNumber = value
                End If
            End If
        End If
        Select Case Me.ErrorLevel
            Case TspErrorLevel.Fatal
                Me.Severity = TraceEventType.Critical
            Case TspErrorLevel.Informational
                Me.Severity = TraceEventType.Information
            Case TspErrorLevel.None
                Me.Severity = TraceEventType.Verbose
            Case TspErrorLevel.Recoverable
                Me.Severity = TraceEventType.Warning
            Case TspErrorLevel.Serious
                Me.Severity = TraceEventType.Error
            Case Else
                Me.Severity = TraceEventType.Verbose
        End Select
    End Sub

#End Region

#Region " TSP ERROR "

    Private _ErrorLevel As TspErrorLevel

    ''' <summary> Gets the error level. </summary>
    ''' <value> The error level. </value>
    Public ReadOnly Property ErrorLevel As TspErrorLevel
        Get
            Return Me._ErrorLevel
        End Get
    End Property

    Private _NodeNumber As Integer

    ''' <summary> Gets the node number. </summary>
    ''' <value> The node number. </value>
    Public ReadOnly Property NodeNumber As Integer
        Get
            Return Me._NodeNumber
        End Get
    End Property

    ''' <summary> Builds error message. </summary>
    ''' <returns> A String. </returns>
    Public Overrides Function BuildErrorMessage() As String
        Return DeviceError.BuildErrorMessage(Me.ErrorNumber, Me.ErrorMessage, Me.ErrorLevel, Me.NodeNumber)
    End Function

    ''' <summary> Builds error message. </summary>
    ''' <returns> A String. </returns>
    Public Overloads Shared Function BuildErrorMessage(ByVal errorNumber As Integer, ByVal errorMessage As String, ByVal errorLevel As TspErrorLevel, ByVal nodeNumber As Integer) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "{0},{1},{2},{3}", errorNumber, errorMessage, CInt(errorLevel), nodeNumber)
    End Function

#End Region


End Class

''' <summary> Enumerates the TSP error levels. </summary>
Public Enum TspErrorLevel
    ''' <summary> Indicates no error: “Queue is Empty”. </summary>
    <ComponentModel.Description("None")> None = 0
    ''' <summary> Indicates an event or a minor error.
    ''' Examples: “Reading Available” and “Reading Overflow”. </summary>
    <ComponentModel.Description("Informational")> Informational = 10
    ''' <summary> Indicates possible invalid user input.
    ''' Operation will continue but action should be taken to correct the error.
    ''' Examples: “Exponent Too Large” and “Numeric Data Not Allowed”. </summary>
    <ComponentModel.Description("Recoverable")> Recoverable = 20
    ''' <summary> Indicates a serious error and may require technical assistance.
    ''' Example: “Saved calibration constants corrupted”. </summary>
    <ComponentModel.Description("Serious")> Serious = 30
    ''' <summary> Indicates that the Series 2600 is non-operational and will
    ''' require service. Contact information for service is provided in Section 1.
    ''' Examples: “Bad SMU A FPGA image size”, “SMU is unresponsive” and
    ''' “Communication Timeout with D FPGA”. </summary>
    <ComponentModel.Description("Fatal")> Fatal = 40
End Enum


