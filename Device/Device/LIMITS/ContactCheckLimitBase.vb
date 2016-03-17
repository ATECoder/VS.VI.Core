Imports isr.Core.Pith
''' <summary> Defines the SCPI Contact Check Limit subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/05/2013" by="David" revision="">            Created based on SCPI 5.1 library. </history>
''' <history date="03/25/2008" by="David" revision="5.0.3004.x">  Port to new SCPI library. </history>
Public MustInherit Class ContactCheckLimitBase
    Inherits NumericLimitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="ContactCheckLimitBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(4, statusSubsystem)
    End Sub

    ''' <summary> Specialized constructor for use only by derived class. </summary>
    ''' <remarks> David, 3/10/2016. </remarks>
    ''' <param name="limitNumber">     The limit number. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Protected Sub New(ByVal limitNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(limitNumber, statusSubsystem)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Enabled = False
        Me.FailureBits = 15
    End Sub

#End Region

#Region " FAILURE BITS "

    Private _FailureBits As Integer?
    ''' <summary> Gets or sets the cached Failure Bits. </summary>
    ''' <value> The Failure Bits or none if not set or unknown. </value>
    Public Overloads Property FailureBits As Integer?
        Get
            Return Me._FailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.FailureBits, value) Then
                Me._FailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FailureBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Failure Bits. </summary>
    ''' <param name="value"> The current Failure Bits. </param>
    ''' <returns> The Failure Bits or none if unknown. </returns>
    Public Function ApplyFailureBits(ByVal value As Integer) As Integer?
        Me.WriteFailureBits(value)
        Return Me.QueryFailureBits()
    End Function

    ''' <summary> Gets Failure Bits query command. </summary>
    ''' <value> The Failure Bits query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM:COMP:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property FailureBitsQueryCommand As String

    ''' <summary> Queries the current Failure Bits. </summary>
    ''' <returns> The Failure Bits or none if unknown. </returns>
    Public Function QueryFailureBits() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.FailureBitsQueryCommand) Then
            Me.FailureBits = Me.Session.Query(0I, Me.BuildCommand(Me.FailureBitsQueryCommand))
        End If
        Return Me.FailureBits
    End Function

    ''' <summary> Gets Failure Bits command format. </summary>
    ''' <value> The Failure Bits command format. </value>
    ''' <remarks> SCPI: ":CALC2:LIM:COMP:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property FailureBitsCommandFormat As String

    ''' <summary> Write the Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Failure Bits. </param>
    ''' <returns> The Failure Bits or none if unknown. </returns>
    Public Function WriteFailureBits(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.FailureBitsCommandFormat) Then
            Me.Session.WriteLine(Me.BuildCommand(Me.FailureBitsCommandFormat), value)
        End If
        Me.FailureBits = value
        Return Me.FailureBits
    End Function

#End Region

End Class
