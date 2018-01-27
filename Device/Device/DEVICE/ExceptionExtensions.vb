Imports System.Text
Imports System.Runtime.CompilerServices

Public Module Methods

#Region " quick access may conflict with ...."

    ''' <summary> Converts a value to a full blown string. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> Value as a String. </returns>
    <Extension>
    Public Function ToFullBlownString(ByVal value As System.Exception) As String
        Return ExceptionExtensions.Methods.ToFullBlownString(value, Integer.MaxValue)
    End Function

#End Region

End Module

Namespace ExceptionExtensions

    ''' <summary> Full blown exception reporting extension methods. </summary>
    ''' <license>
    ''' (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
    ''' Licensed under The MIT License.</para><para>
    ''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
    ''' </license>
    Public Module Methods

#Region " ADD EXCEPTION DATA "

    ''' <summary> Adds an exception data to 'exception'. </summary>
    ''' <param name="value">     The value. </param>
    ''' <param name="exception"> The exception. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Private Function AddExceptionData(ByVal value As System.Exception, ByVal exception As DeviceException) As Boolean
        If exception IsNot Nothing Then
            exception.AddExceptionData(value)
        End If
        Return exception IsNot Nothing
    End Function

    ''' <summary> Adds an exception data to 'exception'. </summary>
    ''' <param name="value">     The value. </param>
    ''' <param name="exception"> The exception. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    Private Function AddExceptionData(ByVal value As System.Exception, ByVal exception As NativeException) As Boolean
        If exception IsNot Nothing AndAlso Not exception.InnerError Is Nothing Then
            exception.AddExceptionData(value)
        End If
        Return exception IsNot Nothing
    End Function

    ''' <summary> Adds an exception data to 'Exception'. </summary>
    ''' <param name="exception"> The exception. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <Extension>
    Public Function AddExceptionData(ByVal exception As System.Exception) As Boolean
        Dim affirmative As Boolean = False
            affirmative = affirmative OrElse isr.Core.Pith.ExceptionExtensions.Methods.AddExceptionData(exception)
            affirmative = affirmative OrElse Methods.AddExceptionData(exception, TryCast(exception, NativeException))
        affirmative = affirmative OrElse Methods.AddExceptionData(exception, TryCast(exception, DeviceException))
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

