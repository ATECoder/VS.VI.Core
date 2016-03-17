Imports isr.Core.Pith
''' <summary> Defines the SCPI Upper/Lower Limit subsystem. </summary>
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
Public MustInherit Class UpperLowerLimitBase
    Inherits NumericLimitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="UpperLowerLimitBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(2, statusSubsystem)
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
        Me.UpperLimit = 1
        Me.UpperLimitFailureBits = 15
        Me.LowerLimit = -1
        Me.LowerLimitFailureBits = 15
        Me.PassBits = 15
    End Sub

#End Region

#Region " PASS BITS "

    Private _PassBits As Integer?
    ''' <summary> Gets the cached Pass Bits. </summary>
    ''' <value> The Pass Bits or none if not set or unknown. </value>
    Public Overloads Property PassBits As Integer?
        Get
            Return Me._PassBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.PassBits, value) Then
                Me._PassBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.PassBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Pass Bits. </summary>
    ''' <param name="value"> The current Pass Bits. </param>
    ''' <returns> The Pass Bits or none if unknown. </returns>
    Public Function ApplyPassBits(ByVal value As Integer) As Integer?
        Me.WritePassBits(value)
        Return Me.QueryPassBits()
    End Function

    ''' <summary> Gets trigger Pass Bits query command. </summary>
    ''' <value> The trigger PassBits query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property PassBitsQueryCommand As String

    ''' <summary> Queries the current pass Bits. </summary>
    ''' <returns> The Compliance Failure Bits or none if unknown. </returns>
    Public Function QueryPassBits() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.PassBitsQueryCommand) Then
            Me.PassBits = Me.Session.Query(0I, Me.BuildCommand(Me.PassBitsQueryCommand))
        End If
        Return Me.PassBits
    End Function

    ''' <summary> Gets Pass Bits command format. </summary>
    ''' <value> The Pass Bits command format. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property PassBitsCommandFormat As String

    ''' <summary> Write the Pass Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Pass Bits. </param>
    ''' <returns> The Pass Bits or none if unknown. </returns>
    Public Function WritePassBits(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.PassBitsCommandFormat) Then
            Me.Session.WriteLine(Me.BuildCommand(Me.PassBitsCommandFormat), value)
        End If
        Me.PassBits = value
        Return Me.PassBits
    End Function

#End Region

#Region " LOWER LIMIT "

    Private _LowerLimit As Double?
    ''' <summary> Gets or sets the cached Lower Limit. </summary>
    ''' <value> The Lower Limit or none if not set or unknown. </value>
    Public Overloads Property LowerLimit As Double?
        Get
            Return Me._LowerLimit
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.LowerLimit, value) Then
                Me._LowerLimit = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LowerLimit))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Lower Limit. </summary>
    ''' <param name="value"> The current Lower Limit. </param>
    ''' <returns> The Lower Limit or none if unknown. </returns>
    Public Function ApplyLowerLimit(ByVal value As Double) As Double?
        Me.WriteLowerLimit(value)
        Return Me.QueryLowerLimit()
    End Function

    ''' <summary> Gets the Lower Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:LOW?" </remarks>
    Protected Overridable ReadOnly Property LowerLimitQueryCommand As String

    ''' <summary> Queries the current Lower Limit. </summary>
    ''' <returns> The Lower Limit or none if unknown. </returns>
    Public Function QueryLowerLimit() As Double?
        Me.LowerLimit = Me.Query(Me.LowerLimit, Me.BuildCommand(Me.LowerLimitQueryCommand))
        Return Me.LowerLimit
    End Function

    ''' <summary> Gets the Lower Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:LOW {0}" </remarks>
    Protected Overridable ReadOnly Property LowerLimitCommandFormat As String

    ''' <summary> Sets the Lower Limit without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit. </param>
    ''' <returns> The Lower Limit or none if unknown. </returns>
    Public Function WriteLowerLimit(ByVal value As Double) As Double?
        Me.LowerLimit = Me.Write(value, Me.BuildCommand(Me.LowerLimitCommandFormat))
        Return Me.LowerLimit
    End Function

#End Region

#Region " LOWER LIMIT FAILURE BITS "

    Private _LowerLimitFailureBits As Integer?
    ''' <summary> Gets or sets the cached Lower Limit Failure Bits. </summary>
    ''' <value> The Lower Limit Failure Bits or none if not set or unknown. </value>
    Public Overloads Property LowerLimitFailureBits As Integer?
        Get
            Return Me._LowerLimitFailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.LowerLimitFailureBits, value) Then
                Me._LowerLimitFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LowerLimitFailureBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Lower Limit Failure Bits. </summary>
    ''' <param name="value"> The current Lower Limit Failure Bits. </param>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function ApplyLowerLimitFailureBits(ByVal value As Integer) As Integer?
        Me.WriteLowerLimitFailureBits(value)
        Return Me.QueryLowerLimitFailureBits()
    End Function

    ''' <summary> Gets the Lower Limit failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:LOW:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property LowerLimitFailureBitsQueryCommand As String

    ''' <summary> Queries the current Lower Limit Failure Bits. </summary>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function QueryLowerLimitFailureBits() As Integer?
        Me.LowerLimitFailureBits = Me.Query(Me.LowerLimitFailureBits, Me.BuildCommand(Me.LowerLimitFailureBitsQueryCommand))
        Return Me.LowerLimitFailureBits
    End Function

    ''' <summary> Gets the Lower Limit Failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:LOW:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property LowerLimitFailureBitsCommandFormat As String

    ''' <summary> Sets back the Lower Limit Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Lower Limit Failure Bits. </param>
    ''' <returns> The Lower Limit Failure Bits or none if unknown. </returns>
    Public Function WriteLowerLimitFailureBits(ByVal value As Integer) As Integer?
        Me.LowerLimitFailureBits = Me.Write(value, Me.BuildCommand(Me.LowerLimitFailureBitsCommandFormat))
        Return Me.LowerLimitFailureBits
    End Function

#End Region

#Region " UPPER LIMIT "

    Private _UpperLimit As Double?
    ''' <summary> Gets or sets the cached Upper Limit. </summary>
    ''' <value> The Upper Limit or none if not set or unknown. </value>
    Public Overloads Property UpperLimit As Double?
        Get
            Return Me._UpperLimit
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.UpperLimit, value) Then
                Me._UpperLimit = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.UpperLimit))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Upper Limit. </summary>
    ''' <param name="value"> The current Upper Limit. </param>
    ''' <returns> The Upper Limit or none if unknown. </returns>
    Public Function ApplyUpperLimit(ByVal value As Double) As Double?
        Me.WriteUpperLimit(value)
        Return Me.QueryUpperLimit()
    End Function

    ''' <summary> Gets the Upper Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:UPP?" </remarks>
    Protected Overridable ReadOnly Property UpperLimitQueryCommand As String

    ''' <summary> Queries the current Upper Limit. </summary>
    ''' <returns> The Upper Limit or none if unknown. </returns>
    Public Function QueryUpperLimit() As Double?
        Me.UpperLimit = Me.Query(Me.UpperLimit, Me.BuildCommand(Me.UpperLimitQueryCommand))
        Return Me.UpperLimit
    End Function

    ''' <summary> Gets the Upper Limit query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:UPP {0}" </remarks>
    Protected Overridable ReadOnly Property UpperLimitCommandFormat As String

    ''' <summary> Sets the Upper Limit without reading back the value from the device. </summary>
    ''' <param name="value"> The current Upper Limit. </param>
    ''' <returns> The Upper Limit or none if unknown. </returns>
    Public Function WriteUpperLimit(ByVal value As Double) As Double?
        Me.UpperLimit = Me.Write(value, Me.BuildCommand(Me.UpperLimitCommandFormat))
        Return Me.UpperLimit
    End Function

#End Region

#Region " UPPER LIMIT FAILURE BITS "

    Private _UpperLimitFailureBits As Integer?
    ''' <summary> Gets or sets the cached Upper Limit Failure Bits. </summary>
    ''' <value> The Upper Limit Failure Bits or none if not set or unknown. </value>
    Public Overloads Property UpperLimitFailureBits As Integer?
        Get
            Return Me._UpperLimitFailureBits
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.UpperLimitFailureBits, value) Then
                Me._UpperLimitFailureBits = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.UpperLimitFailureBits))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Upper Limit Failure Bits. </summary>
    ''' <param name="value"> The current Upper Limit Failure Bits. </param>
    ''' <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    Public Function ApplyUpperLimitFailureBits(ByVal value As Integer) As Integer?
        Me.WriteUpperLimitFailureBits(value)
        Return Me.QueryUpperLimitFailureBits()
    End Function

    ''' <summary> Gets the Upper Limit failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:UPP:SOUR2?" </remarks>
    Protected Overridable ReadOnly Property UpperLimitFailureBitsQueryCommand As String

    ''' <summary> Queries the current Upper Limit Failure Bits. </summary>
    ''' <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    Public Function QueryUpperLimitFailureBits() As Integer?
        Me.UpperLimitFailureBits = Me.Query(Me.UpperLimitFailureBits, Me.BuildCommand(Me.UpperLimitFailureBitsQueryCommand))
        Return Me.UpperLimitFailureBits
    End Function

    ''' <summary> Gets the Upper Limit Failure Bits query command. </summary>
    ''' <value> The Limit enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:LIM2:UPP:SOUR2 {0}" </remarks>
    Protected Overridable ReadOnly Property UpperLimitFailureBitsCommandFormat As String

    ''' <summary> Sets back the Upper Limit Failure Bits without reading back the value from the device. </summary>
    ''' <param name="value"> The current Upper Limit Failure Bits. </param>
    ''' <returns> The Upper Limit Failure Bits or none if unknown. </returns>
    Public Function WriteUpperLimitFailureBits(ByVal value As Integer) As Integer?
        Me.UpperLimitFailureBits = Me.Write(value, Me.BuildCommand(Me.UpperLimitFailureBitsCommandFormat))
        Return Me.UpperLimitFailureBits
    End Function

#End Region

End Class
