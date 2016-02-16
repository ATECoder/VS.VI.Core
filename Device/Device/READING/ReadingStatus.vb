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
        Me._ComplianceBits = CInt(2 ^ 3) ' StatusWordBits.HitCompliance
        Me._Limit1Bits = CInt(2 ^ 9) ' StatusWordBits.LimitResultBit1
        Me._Limit2Bits = CInt(2 ^ 19) ' StatusWordBits.LimitResultBit2
        Me._RangeComplianceBits = CInt(2 ^ 16) ' StatusWordBits.HitRangeCompliance
    End Sub

    ''' <summary> Constructs a copy of an existing value. </summary>
    ''' <param name="model"> The model. </param>
    Public Sub New(ByVal model As ReadingStatus)
        MyBase.New(model)
        If model IsNot Nothing Then
            Me._ComplianceBits = model._ComplianceBits
            Me._Limit1Bits = model._Limit1Bits
            Me._Limit2Bits = model._Limit2Bits
            Me._RangeComplianceBits = model._RangeComplianceBits
        End If
    End Sub

#End Region

#Region " BIT VALUES "

    ''' <summary> Gets the Status Value. </summary>
    ''' <value> The Status Value. </value>
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
    Public Property ComplianceBits() As Integer

    ''' <summary> Gets or sets the bits for detecting limit 1 failure. </summary>
    ''' <value> The limit 1 bits. </value>
    Public Property Limit1Bits() As Integer

    ''' <summary> Gets or sets the bits for detecting limit 2 failure. </summary>
    ''' <value> The limit 2 bits. </value>
    Public Property Limit2Bits() As Integer

    ''' <summary> Gets the bits for detecting range compliance. </summary>
    ''' <value> The range compliance bits. </value>
    Public Property RangeComplianceBits() As Integer

    ''' <summary> Returns an outcome string depending on the measured outcome or pass code. </summary>
    ''' <value> The limit results. </value>
    Public ReadOnly Property LimitResults() As String
        Get
            If Me.Value.HasValue Then

                Select Case Me.StatusValue.Value And (Me.Limit1Bits Or Me.Limit2Bits)
                    Case Me.Limit1Bits
                        Return "F1"
                    Case Me.Limit2Bits
                        Return "F2"
                    Case Me.Limit1Bits Or Me.Limit2Bits
                        Return "F3"
                    Case Else
                        ' 1.11.14 display P and not...
                        Return "P"
                End Select
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary> Gets the condition for hit real compliance. </summary>
    ''' <value> A <see cref="System.Boolean">Boolean</see> value. </value>
    Public ReadOnly Property IsHitCompliance() As Boolean
        Get
            Return (Me.StatusValue.GetValueOrDefault(0) And Me.ComplianceBits) <> 0
        End Get
    End Property

    ''' <summary> Gets the condition for hit range compliance. </summary>
    ''' <value> A <see cref="System.Boolean">Boolean</see> value. </value>
    Public ReadOnly Property IsHitRangeCompliance() As Boolean
        Get
            Return (Me.StatusValue.GetValueOrDefault(0) And Me.RangeComplianceBits) <> 0
        End Get
    End Property

#End Region

End Class

