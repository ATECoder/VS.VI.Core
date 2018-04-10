Imports isr.Core.Pith
''' <summary> Defines the SCPI numeric limit subsystem base. </summary>
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
Public MustInherit Class NumericLimitBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="NumericLimitBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal limitNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._LimitNumber = limitNumber
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
    End Sub

#End Region

#Region " NUMERIC COMMAND BUILDER "

    ''' <summary> Gets or sets the limit number. </summary>
    ''' <value> The limit number. </value>
    Protected ReadOnly Property LimitNumber As Integer

    ''' <summary> Builds a command. </summary>
    ''' <param name="baseCommand"> The base command. </param>
    ''' <returns> A String. </returns>
    Protected Function BuildCommand(ByVal baseCommand As String) As String
        Return String.Format(Globalization.CultureInfo.InvariantCulture, baseCommand, Me.LimitNumber)
    End Function

#End Region

#Region " LIMIT FAILED "

    Private _Failed As Boolean?
    ''' <summary> Gets or sets the cached In Limit Failed Condition sentinel. </summary>
    ''' <value> <c>null</c> if In Limit Failed Condition is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Failed As Boolean?
        Get
            Return Me._Failed
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Failed, value) Then
                Me._Failed = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the Limit Failed query command. </summary>
    ''' <value> The Limit Failed query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM[#]:FAIL?" </remarks>
    Protected Overridable ReadOnly Property FailedQueryCommand As String

    ''' <summary> Queries the Auto Delay Failed sentinel. Also sets the
    ''' <see cref="Failed">Failed</see> sentinel. </summary>
    ''' <returns> <c>True</c> if Failed; otherwise <c>False</c>. </returns>
    Public Function QueryFailed() As Boolean?
        Me.Failed = Me.Query(Me.Failed, Me.BuildCommand(Me.FailedQueryCommand))
        Return Me.Failed
    End Function

#End Region

#Region " LIMIT ENABLED "

    Private _Enabled As Boolean?
    ''' <summary> Gets the cached Limit Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Limit Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Enabled As Boolean?
        Get
            Return Me._Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Enabled, value) Then
                Me._Enabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if Enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteEnabled(value)
        Return Me.QueryEnabled()
    End Function

    ''' <summary> Gets the Limit enabled query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "CALC2:LIM[#]:STAT?" </remarks>
    Protected Overridable ReadOnly Property EnabledQueryCommand As String

    ''' <summary> Queries the Auto Delay Enabled sentinel. Also sets the
    ''' <see cref="Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryEnabled() As Boolean?
        Me.Enabled = Me.Query(Me.Enabled, Me.BuildCommand(Me.EnabledQueryCommand))
        Return Me.Enabled
    End Function

    ''' <summary> Gets the Limit enabled command Format. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: "CALC2:LIM[#]:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property EnabledCommandFormat As String

    ''' <summary> Writes the Auto Delay Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteEnabled(ByVal value As Boolean) As Boolean?
        Me.Enabled = Me.Write(value, Me.BuildCommand(Me.EnabledCommandFormat))
        Return Me.Enabled
    End Function

#End Region

End Class

