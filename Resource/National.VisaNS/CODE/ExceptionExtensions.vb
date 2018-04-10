Imports System.Text
Imports System.Runtime.CompilerServices
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
    Private Function AddExceptionData(ByVal value As System.Exception, ByVal exception As NationalInstruments.VisaNS.VisaException) As Boolean
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
        affirmative = affirmative OrElse Methods.AddExceptionData(exception, TryCast(exception, NationalInstruments.VisaNS.VisaException))
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

    ''' <summary> Appends a full blown string. </summary>
    ''' <param name="builder"> The builder. </param>
    ''' <param name="value">   The value. </param>
    ''' <param name="prefix">  The prefix. </param>
    Private Sub AppendFullBlownString(ByVal builder As StringBuilder, ByVal value As System.Exception, ByVal prefix As String)
        builder.AppendLine($"{prefix}Type: {value.GetType}")
        If Not String.IsNullOrEmpty(value.Message) Then builder.AppendLine($"{prefix}Message: {value.Message}")
        If Not String.IsNullOrEmpty(value.Source) Then builder.AppendLine($"{prefix}Source: {value.Source}")
        If value.TargetSite IsNot Nothing Then builder.AppendLine($"{prefix}Method: {value.TargetSite}")
        If Not String.IsNullOrEmpty(value.StackTrace) Then
            For Each s As String In value.StackTrace.Split(Environment.NewLine.ToCharArray)
                s = s.Trim
                If Not String.IsNullOrWhiteSpace(s) Then
                    builder.AppendLine($"{prefix}{s.Replace(" in ", $"{Environment.NewLine}{prefix}in ")}")
                End If
            Next
        End If
        If value.Data IsNot Nothing Then
            For Each keyValuePair As System.Collections.DictionaryEntry In value.Data
                builder.AppendLine($"{prefix} Data: {keyValuePair.Key}: {keyValuePair.Value}")
            Next
        End If
    End Sub

    ''' <summary> Converts this object to a full blown string. </summary>
    ''' <param name="value"> The value. </param>
    ''' <param name="level"> The level. </param>
    ''' <returns> The given data converted to a String. </returns>
    <System.Runtime.CompilerServices.Extension>
    Public Function ToFullBlownString(ByVal value As System.Exception, ByVal level As Integer) As String
        Dim builder As New StringBuilder()
        Dim counter As Integer = 1
        builder.AppendLine()
        Do While value IsNot Nothing AndAlso counter <= level
            Methods.AddExceptionData(value)
            Methods.AppendFullBlownString(builder, value, $"{counter}-> ")
            value = value.InnerException
            counter += 1
        Loop
        Return builder.ToString().TrimEnd(Environment.NewLine.ToCharArray)
    End Function

#End Region

End Module
