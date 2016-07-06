''' <summary> Defines the contract that must be implemented by a Measure Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class MeasureSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="MeasureSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets subsystem values to their known execution clear state. </summary>
    Public Overrides Sub ClearExecutionState()
        MyBase.ClearExecutionState()
        Me.MeasurementAvailable = False
    End Sub

#End Region

#Region "  INIT, READ, FETCH "

    Private _LastReading As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastReading As String
        Get
            Return Me._LastReading
        End Get
        Set(ByVal value As String)
            Me._LastReading = value
            Me.AsyncNotifyPropertyChanged(NameOf(Me.LastReading))
        End Set
    End Property

    ''' <summary> Parses the reading into the data elements. </summary>
    Public MustOverride Sub ParseReading(ByVal reading As String)

    ''' <summary> Gets the fetch command. </summary>
    ''' <value> The fetch command. </value>
    ''' <remarks> SCPI: 'FETCh?' </remarks>
    Protected Overridable ReadOnly Property FetchCommand As String

    ''' <summary> Fetches the data. </summary>
    ''' <remarks> Issues the 'FETCH?' query, which reads data stored in the Sample Buffer. If, for example, there
    ''' are 20 data arrays stored in the Sample Buffer, then all 20 data arrays will be sent to the
    ''' computer when 'FETCh?' is executed. Note that FETCh? does not affect data in the Sample Buffer.
    ''' Thus, subsequent executions of FETCh? acquire the same data. </remarks>
    Public Overridable Sub Fetch()
        Me.LastReading = Me.Query(Me.Session.EmulatedReply, Me.FetchCommand)
        Me.ParseReading(Me.LastReading)
        Me.MeasurementAvailable = True
    End Sub

    ''' <summary> Gets the read command. </summary>
    ''' <value> The read command. </value>
    ''' <remarks> SCPI: 'READ' </remarks>
    Protected Overridable ReadOnly Property ReadCommand As String

    ''' <summary> Initiates an operation and then fetches the data. </summary>
    ''' <remarks> Issues the 'READ?' query, which performs a trigger initiation and then a
    ''' <see cref="Fetch">FETCh? </see>
    ''' The initiate triggers a new measurement cycle which puts new data in the Sample Buffer. Fetch
    ''' reads that new data. The <see cref="MeasureCurrentSubsystemBase.Measure">Measure</see> command
    ''' places the instrument in a “one-shot” mode and then performs a read. </remarks>
    Public Overridable Sub Read()
        Me.LastReading = Me.Query(Me.Session.EmulatedReply, Me.ReadCommand)
        Me.ParseReading(Me.LastReading)
        Me.MeasurementAvailable = True
    End Sub

    ''' <summary> <c>True</c> if Measurement available. </summary>
    Private _MeasurementAvailable As Boolean

    ''' <summary> Gets or sets a value indicating whether [Measurement available]. </summary>
    ''' <value> <c>True</c> if [Measurement available]; otherwise, <c>False</c>. </value>
    Public Property MeasurementAvailable As Boolean
        Get
            Return Me._MeasurementAvailable
        End Get
        Protected Set(ByVal value As Boolean)
            Me._MeasurementAvailable = value
            ' Me.SyncNotifyPropertyChanged(NameOf(me.MeasurementAvailable))
            Me.SafeInvokePropertyChanged(NameOf(Me.MeasurementAvailable))
        End Set
    End Property

    ''' <summary> Asynchronous measurement available setter. </summary>
    ''' <param name="value"> <c>True</c> to indicate that a measurement is available; Otherwise, <c>False</c>. </param>
    Public Sub AsyncMeasurementAvailable(ByVal value As Boolean)
        Me._MeasurementAvailable = value
        Me.AsyncNotifyPropertyChanged(NameOf(Me.MeasurementAvailable))
    End Sub

#End Region

End Class

#Region " UNUSED "
#If False Then

    ''' <summary> Gets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    ''' <remarks> SCPI: :INIT'. </remarks>
    Protected Overridable ReadOnly Property InitiateCommand As String

    ''' <summary> Initiates operation. </summary>
    ''' <remarks> issues the ':INIT' command. </remarks>
    Public Sub Initiate()
        Me.Session.Execute(Me.InitiateCommand)
    End Sub

#End If
#End Region