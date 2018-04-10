Imports System.Text
Imports System.Runtime.CompilerServices
Namespace ExceptionExtensions

    ''' <summary> Includes extensions for <see cref="Exception">Exceptions</see>. </summary>
    ''' <license> (c) 2017 Marco Bertschi.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="03/30/2017" by="David" revision="3.1.6298.x">
    ''' https://www.codeproject.com/script/Membership/View.aspx?mid=8888914
    ''' https://www.codeproject.com/Tips/1179564/A-Quick-Dirty-Extension-Method-to-Get-the-Full-Exc
    ''' </history>
    Public Module Methods

#Region " ADD EXCEPTION DATA "

        ''' <summary> Adds an exception data to 'exception'. </summary>
        ''' <param name="value">     The value. </param>
        ''' <param name="exception"> The exception. </param>
        ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
        Private Function AddExceptionData(ByVal value As System.Exception, ByVal exception As VI.Pith.DeviceException) As Boolean
            If exception IsNot Nothing Then
                exception.AddExceptionData(value)
            End If
            Return exception IsNot Nothing
        End Function

        ''' <summary> Adds an exception data to 'exception'. </summary>
        ''' <param name="value">     The value. </param>
        ''' <param name="exception"> The exception. </param>
        ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
        Private Function AddExceptionData(ByVal value As System.Exception, ByVal exception As VI.Pith.NativeException) As Boolean
            If exception IsNot Nothing AndAlso Not exception.InnerError Is Nothing Then
                exception.AddExceptionData(value)
            End If
            Return exception IsNot Nothing
        End Function

        ''' <summary> Adds an exception data to 'exception'. </summary>
        ''' <param name="value">     The value. </param>
        ''' <param name="exception"> The exception. </param>
        ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
        Private Function AddExceptionData(ByVal value As System.Exception, ByVal exception As Ivi.Visa.NativeVisaException) As Boolean
            If exception IsNot Nothing Then
                If exception.ErrorCode > 0 Then
                    value.Data.Add($"{value.Data.Count}-Warning", $"0x{exception.ErrorCode:X}")
                ElseIf exception.ErrorCode < 0 Then
                    value.Data.Add($"{value.Data.Count}-Error", $"-0x{-exception.ErrorCode:X}")
                End If
            End If
            Return exception IsNot Nothing
        End Function

        ''' <summary> Adds an exception data to 'Exception'. </summary>
        ''' <param name="exception"> The exception. </param>
        ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
        <Extension>
        Public Function AddExceptionData(ByVal exception As System.Exception) As Boolean
            Dim affirmative As Boolean = False
            affirmative = affirmative OrElse isr.Core.Pith.ExceptionExtensions.AddExceptionData(exception)
            affirmative = affirmative OrElse Methods.AddExceptionData(exception, TryCast(exception, Ivi.Visa.NativeVisaException))
            affirmative = affirmative OrElse Methods.AddExceptionData(exception, TryCast(exception, VI.Pith.NativeException))
            affirmative = affirmative OrElse Methods.AddExceptionData(exception, TryCast(exception, VI.Pith.DeviceException))
            Return affirmative
        End Function

#End Region

#Region " TO FULL BLOWN STRING "

        ''' <summary> Converts a value to a full blown string. </summary>
        ''' <param name="value"> The value. </param>
        ''' <returns> Value as a String. </returns>
        <System.Runtime.CompilerServices.Extension>
        Public Function ToFullBlownString(ByVal value As System.Exception) As String
            Return Methods.ToFullBlownString(value, Integer.MaxValue)
        End Function

        ''' <summary> Converts this object to a full blown string. </summary>
        ''' <param name="value"> The value. </param>
        ''' <param name="level"> The level. </param>
        ''' <returns> The given data converted to a String. </returns>
        <System.Runtime.CompilerServices.Extension>
        Public Function ToFullBlownString(ByVal value As System.Exception, ByVal level As Integer) As String
            Return isr.Core.Pith.ExceptionExtensions.Methods.ToFullBlownString(value, level, AddressOf Methods.AddExceptionData)
        End Function

#End Region

    End Module
End Namespace
