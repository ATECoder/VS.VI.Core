''' <summary> Defines a <see cref="System.Int32">Status</see> reading. </summary>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/1/2013" by="David" revision=""> Created. </history>
Public Class ReadingStatus
    Inherits ReadingValue

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As ReadingStatus)
        MyBase.New(model)
    End Sub

#End Region

#Region " VALUES "

    ''' <summary> Gets the Status Value. </summary>
    ''' <value> The Status Value. </value>
    ''' <remarks> Handles the case where the status value was saved as infinite. </remarks>
    Public ReadOnly Property StatusValue As Long?
        Get
            If Me.Value.HasValue Then
                Return CLng(Math.Min(Math.Max(0, Me.Value.Value), Long.MaxValue))
            Else
                Return New Integer?
            End If
        End Get
    End Property

    ''' <summary> Query if 'bit' is bit. </summary>
    ''' <remarks> David, 3/7/2016. </remarks>
    ''' <param name="bit"> The bit. </param>
    ''' <returns> <c>true</c> if bit; otherwise <c>false</c> </returns>
    Public Function IsBit(ByVal bit As Integer) As Boolean
        Return (1 And (Me.StatusValue.GetValueOrDefault(0) And (1 >> bit))) = 1
    End Function

#End Region

End Class

#Region " UNUSED "
#If False Then
#Region " BIT VALUES "

    ''' <summary> Gets the Status Value. </summary>
    ''' <value> The Status Value. </value>
    ''' <remarks> Handles the case where the status value was saved as infinite. </remarks>
    Public ReadOnly Property StatusValue As Long?
        Get
            If Me.Value.HasValue Then
                Return CLng(Math.Min(Math.Max(0, Me.Value.Value), Long.MaxValue))
            Else
                Return New Integer?
            End If
        End Get
    End Property

    ''' <summary> Gets or sets the bits for detecting compliance. </summary>
    ''' <value> The compliance bits. </value>
    Public Property ComplianceBit() As Integer

    ''' <summary> Gets or sets the bits for detecting limit 1 failure. </summary>
    ''' <value> The limit 1 bits. </value>
    Public Property Limit1Bit() As Integer

    ''' <summary> Gets or sets the bits for detecting limit 2 failure. </summary>
    ''' <value> The limit 2 bits. </value>
    Public Property Limit2Bit() As Integer

    ''' <summary> Gets the bits for detecting range compliance. </summary>
    ''' <value> The range compliance bits. </value>
    Public Property RangeComplianceBit() As Integer

    ''' <summary> Returns an outcome string depending on the measured outcome or pass code. </summary>
    ''' <value> The limit results. </value>
    Public ReadOnly Property LimitResults() As String
        Get
            If Me.Value.HasValue Then
                Dim f1 As Boolean = Me.IsBit(Me.Limit1Bit)
                Dim f2 As Boolean = Me.IsBit(Me.Limit2Bit)
                If f1 AndAlso f2 Then
                    Return "F3"
                ElseIf f1 Then
                    Return "F1"
                ElseIf f2 Then
                    Return "F2"
                Else
                    ' 1.11.14 display P and not...
                    Return "P"
                End If
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary> Query if 'bit' is bit. </summary>
    ''' <remarks> David, 3/7/2016. </remarks>
    ''' <param name="bit"> The bit. </param>
    ''' <returns> <c>true</c> if bit; otherwise <c>false</c> </returns>
    Public Function IsBit(ByVal bit As Integer) As Boolean
        Return (1 And (Me.StatusValue.GetValueOrDefault(0) And (1 >> bit))) = 1
    End Function

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As ReadingStatus)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._ComplianceBit = model._ComplianceBit
            Me._Limit1Bit = model._Limit1Bit
            Me._Limit2Bit = model._Limit2Bit
            Me._RangeComplianceBit = model._RangeComplianceBit
        End If
    End Sub

#End Region
#End If
#End Region