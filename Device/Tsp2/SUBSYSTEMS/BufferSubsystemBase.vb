
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
Public MustInherit Class BufferSubsystemBase
    Inherits VI.BufferSubsystemBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="BufferSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        Me.New(BufferSubsystemBase.DefaultBuffer1Name, statusSubsystem)
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <param name="bufferName">      The name of the buffer. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal bufferName As String, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.BufferName = bufferName
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
    End Sub

#End Region

#Region " BUFFER NAME "

    ''' <summary> The default buffer 1 name. </summary>
    Public Const DefaultBuffer1Name As String = "defbuffer1"

    ''' <summary> The default buffer 2 name. </summary>
    Public Const DefaultBuffer2Name As String = "defbuffer2"

    Private _BufferName As String

    ''' <summary> Gets or sets the name of the buffer. </summary>
    ''' <value> The name of the buffer. </value>
    Public Property BufferName As String
        Get
            Return Me._BufferName
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.BufferName) Then
                Me._BufferName = If(String.IsNullOrWhiteSpace(value), BufferSubsystemBase.DefaultBuffer1Name, value)
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

#End Region

#Region " COMMAND SYNTAX "

#Region " CLEAR "

    ''' <summary> Gets or sets the Clear Buffer command. </summary>
    ''' <value> The ClearBuffer command. </value>
    Protected Overrides ReadOnly Property ClearBufferCommand As String
        Get
            Return $"{Me.BufferName}.clear"
        End Get
    End Property


#End Region

#Region " CAPACITY "

    ''' <summary> Gets the points count query command. </summary>
    ''' <value> The points count query command. </value>
    Protected Overrides ReadOnly Property CapacityQueryCommand As String
        Get
            Return $"_G.print(String.Format('%d',{Me.BufferName}.capacity))"
        End Get
    End Property

    ''' <summary> Gets the points count command format. </summary>
    ''' <remarks> SCPI: ":TRAC:POIN:COUN {0}". </remarks>
    ''' <value> The points count query command format. </value>
    Protected Overrides ReadOnly Property CapacityCommandFormat As String
        Get
            Return $"{Me.BufferName}.capacity={{0}}"
        End Get
    End Property

#End Region

#Region " FILL ONCE ENABLED  "

    ''' <summary> Gets or sets the automatic Delay enabled query command. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overrides ReadOnly Property FillOnceEnabledQueryCommand As String
        Get
            Return $"_G.print({Me.BufferName}.fillmode=buffer.FILL_ONCE"
        End Get
    End Property

    ''' <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    ''' <value> The automatic Delay enabled query command. </value>
    Protected Overrides ReadOnly Property FillOnceEnabledCommandFormat As String
        Get
            Return $"{Me.BufferName}.fillmode={0:'buffer.FILL_ONCE';'buffer.FILL_ONCE';'buffer.FILL_CONTINUOUS'}"
        End Get
    End Property

#End Region

#Region " ACTUAL POINTS "

    ''' <summary> Gets or sets the ActualPoint count query command. </summary>
    ''' <value> The ActualPoint count query command. </value>
    Protected Overrides ReadOnly Property ActualPointCountQueryCommand As String = $"_G.print(String.Format('%d',{Me.BufferName}.n))"

    ''' <summary> Gets or sets The First Point Number (1-based) query command. </summary>
    ''' <value> The First Point Number query command. </value>
    Protected Overrides ReadOnly Property FirstPointNumberQueryCommand As String = $"_G.print(String.Format('%d',{Me.BufferName}.startindex))"

    ''' <summary> Gets or sets The Last Point Number (1-based) query command. </summary>
    ''' <value> The Last Point Number query command. </value>
    Protected Overrides ReadOnly Property LastPointNumberQueryCommand As String = $"_G.print(String.Format('%d',{Me.BufferName}.endindex))"

#End Region

#Region " BUFFER STREAMING "

    ''' <summary> Gets the buffer read command format. </summary>
    ''' <value> The buffer read command format. </value>
    Public Overrides ReadOnly Property BufferReadCommandFormat As String
        Get
            Return $"_G.printbuffer({{0}},{{1}},{Me.BufferName}.readings,{Me.BufferName}.relativetimestampts,{Me.BufferName}.statuses,{Me.BufferName}.units"
        End Get
    End Property

#End Region

#End Region

#Region " DATA "

    ''' <summary> Gets or sets the data query command. </summary>
    ''' <value> The points count query command. </value>
    Protected Overrides ReadOnly Property DataQueryCommand As String = ""

#End Region

End Class

