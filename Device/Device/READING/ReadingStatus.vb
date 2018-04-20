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

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Default constructor. </summary>
    Public Sub New(ByVal readingType As ReadingTypes)
        MyBase.New(readingType)
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
                Return CLng(If(Me.Value.Value < 0, 0, If(Me.Value.Value > Long.MaxValue, Long.MaxValue, Me.Value.Value)))
            Else
                Return New Integer?
            End If
        End Get
    End Property

    ''' <summary> Query if 'bit' is bit. </summary>
    ''' <param name="bit"> The bit. </param>
    ''' <returns> <c>true</c> if bit; otherwise <c>false</c> </returns>
    Public Function IsBit(ByVal bit As Integer) As Boolean
        Return Me.StatusValue.HasValue AndAlso (1 And (Me.StatusValue.Value >> bit)) = 1
    End Function

    ''' <summary> Returns a string that represents the current object. </summary>
    ''' <returns> A string that represents the current object. </returns>
    Public Overrides Function ToString() As String
        If Me.StatusValue.HasValue Then
            Return String.Format("0x{0:X}", Me.StatusValue.Value)
        Else
            Return "empty"
        End If
    End Function

    Public Overloads Function ToString(ByVal nibbleCount As Integer) As String
        If Me.StatusValue.HasValue Then
            Return String.Format(String.Format("0x{{0:X{0}}}", nibbleCount), Me.StatusValue.Value)
        Else
            Return Me.ToString
        End If
    End Function

#End Region

End Class

