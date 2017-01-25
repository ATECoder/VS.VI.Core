''' <summary> Defines the contract that must be implemented by a Trace Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class TraceSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="TraceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.PointsCount = 0
    End Sub

#End Region

#Region " COMMANDS "

    ''' <summary> Gets or sets the Clear Buffer command. </summary>
    ''' <value> The ClearBuffer command. </value>
    ''' <remarks> SCPI: ":TRAC:CLE". </remarks>
    Protected Overridable ReadOnly Property ClearBufferCommand As String

    ''' <summary> Clears the buffer. </summary>
    ''' <remarks> David, 6/25/2016. </remarks>
    Public Sub ClearBuffer()
        Me.Write(Me.ClearBufferCommand)
    End Sub

#End Region

#Region " POINTS COUNT "

    ''' <summary> Number of points. </summary>
    Private _PointsCount As Integer?

    ''' <summary> Gets or sets the cached Trace PointsCount. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Trace model. </remarks>
    ''' <value> The Trace PointsCount or none if not set or unknown. </value>
    Public Overloads Property PointsCount As Integer?
        Get
            Return Me._PointsCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.PointsCount, value) Then
                Me._PointsCount = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.PointsCount))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Trace PointsCount. </summary>
    ''' <param name="value"> The current PointsCount. </param>
    ''' <returns> The PointsCount or none if unknown. </returns>
    Public Function ApplyPointsCount(ByVal value As Integer) As Integer?
        Me.WritePointsCount(value)
        Return Me.QueryPointsCount()
    End Function

    ''' <summary> Gets or sets the points count query command. </summary>
    ''' <value> The points count query command. </value>
    ''' <remarks> SCPI: ":TRAC:POIN:COUN?" </remarks>
    Protected Overridable ReadOnly Property PointsCountQueryCommand As String

    ''' <summary> Queries the current PointsCount. </summary>
    ''' <returns> The PointsCount or none if unknown. </returns>
    Public Function QueryPointsCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.PointsCountQueryCommand) Then
            Me.PointsCount = Me.Session.Query(0I, Me.PointsCountQueryCommand)
        End If
        Return Me.PointsCount
    End Function

    ''' <summary> Gets or sets the points count command format. </summary>
    ''' <value> The points count query command format. </value>
    ''' <remarks> SCPI: ":TRAC:POIN:COUN {0}" </remarks>
    Protected Overridable ReadOnly Property PointsCountCommandFormat As String

    ''' <summary> Write the Trace PointsCount without reading back the value from the device. </summary>
    ''' <param name="value"> The current PointsCount. </param>
    ''' <returns> The PointsCount or none if unknown. </returns>
    Public Function WritePointsCount(ByVal value As Integer) As Integer?
        If Not String.IsNullOrWhiteSpace(Me.PointsCountCommandFormat) Then
            Me.Session.WriteLine(Me.PointsCountCommandFormat, value)
        End If
        Me.PointsCount = value
        Return Me.PointsCount
    End Function

#End Region

#Region " ACTUAL POINT COUNT "

    ''' <summary> Number of ActualPoint. </summary>
    Private _ActualPointCount As Integer?

    ''' <summary> Gets or sets the cached Trace ActualPointCount. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Trace model. </remarks>
    ''' <value> The Trace ActualPointCount or none if not set or unknown. </value>
    Public Overloads Property ActualPointCount As Integer?
        Get
            Return Me._ActualPointCount
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.ActualPointCount, value) Then
                Me._ActualPointCount = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ActualPointCount))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the ActualPoint count query command. </summary>
    ''' <value> The ActualPoint count query command. </value>
    ''' <remarks> SCPI: ":TRAC:ACT?" </remarks>
    Protected Overridable ReadOnly Property ActualPointCountQueryCommand As String

    ''' <summary> Queries the current ActualPointCount. </summary>
    ''' <returns> The ActualPointCount or none if unknown. </returns>
    Public Function QueryActualPointCount() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.ActualPointCountQueryCommand) Then
            Me.ActualPointCount = Me.Session.Query(0I, Me.ActualPointCountQueryCommand)
        End If
        Return Me.ActualPointCount
    End Function

#End Region

#Region " FIRST POINT NUMBER "

    ''' <summary> Number of First Point. </summary>
    Private _FirstPointNumber As Integer?

    ''' <summary> Gets or sets the cached buffer First Point Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Trace model. </remarks>
    ''' <value> The buffer First Point Number or none if not set or unknown. </value>
    Public Overloads Property FirstPointNumber As Integer?
        Get
            Return Me._FirstPointNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.FirstPointNumber, value) Then
                Me._FirstPointNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FirstPointNumber))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets The First Point Number query command. </summary>
    ''' <value> The First Point Number query command. </value>
    ''' <remarks> SCPI: ":TRAC:ACT:STA?" </remarks>
    Protected Overridable ReadOnly Property FirstPointNumberQueryCommand As String

    ''' <summary> Queries the current FirstPointNumber. </summary>
    ''' <returns> The First Point Number or none if unknown. </returns>
    Public Function QueryFirstPointNumber() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.FirstPointNumberQueryCommand) Then
            Me.FirstPointNumber = Me.Session.Query(0I, Me.FirstPointNumberQueryCommand)
        End If
        Return Me.FirstPointNumber
    End Function

#End Region

#Region " LAST POINT NUMBER "

    ''' <summary> Number of Last Point. </summary>
    Private _LastPointNumber As Integer?

    ''' <summary> Gets or sets the cached buffer Last Point Number. </summary>
    ''' <remarks> Specifies how many times an operation is performed in the specified layer of the
    ''' Trace model. </remarks>
    ''' <value> The buffer Last Point Number or none if not set or unknown. </value>
    Public Overloads Property LastPointNumber As Integer?
        Get
            Return Me._LastPointNumber
        End Get
        Protected Set(ByVal value As Integer?)
            If Not Nullable.Equals(Me.LastPointNumber, value) Then
                Me._LastPointNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.LastPointNumber))
            End If
        End Set
    End Property

    ''' <summary> Gets or sets The Last Point Number query command. </summary>
    ''' <value> The Last Point Number query command. </value>
    ''' <remarks> SCPI: ":TRAC:ACT:END?" </remarks>
    Protected Overridable ReadOnly Property LastPointNumberQueryCommand As String

    ''' <summary> Queries the current Last Point Number. </summary>
    ''' <returns> The LastPointNumber or none if unknown. </returns>
    Public Function QueryLastPointNumber() As Integer?
        If Not String.IsNullOrWhiteSpace(Me.LastPointNumberQueryCommand) Then
            Me.LastPointNumber = Me.Session.Query(0I, Me.LastPointNumberQueryCommand)
        End If
        Return Me.LastPointNumber
    End Function

#End Region

#Region " DATA "

    ''' <summary> String. </summary>
    Private _Data As String

    ''' <summary> Gets or sets the cached Trace Data. </summary>
    Public Overloads Property Data As String
        Get
            Return Me._Data
        End Get
        Protected Set(ByVal value As String)
            If Not Nullable.Equals(Me.Data, value) Then
                Me._Data = value
                Me.AsyncNotifyPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the data query command. </summary>
    ''' <value> The points count query command. </value>
    ''' <remarks> SCPI: ":TRAC:DATA?" </remarks>
    Protected Overridable ReadOnly Property DataQueryCommand As String

    ''' <summary> Queries the current Data. </summary>
    ''' <returns> The Data or none if unknown. </returns>
    Public Function QueryData() As String
        If Not String.IsNullOrWhiteSpace(Me.DataQueryCommand) Then
            Me.Session.WriteLine(Me.DataQueryCommand)
            ' read the entire data.
            Me.Data = Me.Session.ReadFreeLineTrimEnd()
        End If
        Return Me.Data
    End Function

    ''' <summary> Queries the current Data. </summary>
    ''' <returns> The Data or empty if none. </returns>
    Public Function QueryData(ByVal queryCommand As String) As String
        If Not String.IsNullOrWhiteSpace(queryCommand) Then
            Me.Data = ""
            Me.Session.WriteLine(queryCommand)
            ' read the entire data.
            Me.Data = Me.Session.ReadFreeLineTrimEnd()
        End If
        Return Me.Data
    End Function

#End Region

End Class

#Region " UNUSED "
#If False Then
' part of trigger subsystem

#Region " COMMANDS "

    ''' <summary> Gets or sets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    ''' <remarks> SCPI: ":INIT". </remarks>
    Protected Overridable ReadOnly Property InitiateCommand As String

    ''' <summary> Initiates operations. </summary>
    ''' <remarks> This command is used to initiate source-measure operation by taking the SourceMeter
    ''' out of idle. The :READ? and :MEASure? commands also perform an initiation. Note that if auto
    ''' output-off is disabled (SOURce1:CLEar:AUTO OFF), the source output must first be turned on
    ''' before an initiation can be performed. The :MEASure? command automatically turns the output
    ''' source on before performing the initiation. </remarks>
    Public Sub Initiate()
        If Not String.IsNullOrWhiteSpace(Me.InitiateCommand) Then
            Me.Session.WriteLine(Me.InitiateCommand)
        End If
    End Sub

    ''' <summary> Gets or sets the Abort command. </summary>
    ''' <value> The Abort command. </value>
    ''' <remarks> SCPI: ":ABOR". </remarks>
    Protected Overridable ReadOnly Property AbortCommand As String

    ''' <summary> Aborts operations. </summary>
    ''' <remarks> When this action command is sent, the SourceMeter aborts operation and returns to the
    ''' idle state. A faster way to return to idle is to use the DCL or SDC command. With auto output-
    ''' off enabled (:SOURce1:CLEar:AUTO ON), the output will remain on if operation is terminated
    ''' before the output has a chance to automatically turn off. </remarks>
    Public Sub Abort()
        If Not String.IsNullOrWhiteSpace(Me.AbortCommand) Then
            Me.Session.WriteLine(Me.AbortCommand)
        End If
    End Sub

#End Region


#End If

#End Region
