''' <summary> A device error. </summary>
''' <remarks> David, 1/12/2016. </remarks>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="1/12/2016" by="David" revision=""> Created. </history>
Public Class DeviceError
    Inherits VI.DeviceError

#Region " CONSTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="DeviceError" /> class 
    '''           specifying no error. </summary>
    Public Sub New()
        MyBase.New("0,No Errors")
    End Sub

#End Region

#Region " PARSE "

    Public Overrides Sub Parse(ByVal compoundError As String)
        MyBase.Parse(compoundError)
        If Not String.IsNullOrWhiteSpace(compoundError) Then
            ' parse EG Prober Errors.
            If compoundError.StartsWith("E", StringComparison.OrdinalIgnoreCase) Then
                If Integer.TryParse(compoundError.Substring(1), Me.ErrorNumber) Then
                End If
                Me.ErrorMessage = compoundError
            End If
        End If
    End Sub

#End Region

End Class
