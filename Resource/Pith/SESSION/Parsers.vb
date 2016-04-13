Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
Partial Public Class SessionBase

#Region " PARSERS "

#Region " BOOLEAN "

    ''' <summary> Parses a value to a Boolean. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy"> A dummy that distinguishes this method. </param>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>True</c> if the value equals '1' or <c>False</c> if '0'; otherwise an exception is thrown. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="dummy")>
    Public Shared Function Parse(ByVal dummy As Boolean, ByVal value As String) As Boolean
        If TryParse(value, dummy) Then
            Return dummy
        Else
            Throw New FormatException($"'{value}' is not a valid Boolean value")
        End If
    End Function

    ''' <summary> Converts a value to an one zero. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The value. </param>
    ''' <returns> value as a String. </returns>
    Public Shared Function ToOneZero(ByVal value As Boolean) As String
        Return $"{value.GetHashCode:'1';'1';'0'}"
    End Function

    ''' <summary> Converts a value to a true false. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="value"> The value. </param>
    ''' <returns> value as a String. </returns>
    Public Shared Function ToTrueFalse(ByVal value As Boolean) As String
        Return $"{value.GetHashCode:'true';'true';'false'}"
    End Function

    ''' <summary> Tries to parse a value to a Boolean. </summary>
    ''' <param name="value"> The value. </param>
    ''' <param name="result">   [in,out] Value read from the instrument. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryParse(ByVal value As String, ByRef result As Boolean) As Boolean
        Dim numericValue As Integer = 0
        Dim returnValue As Boolean = False
        If String.IsNullOrWhiteSpace(value) Then
            returnValue = False
        ElseIf Integer.TryParse(value, numericValue) Then
            result = numericValue <> 0
            returnValue = result
        ElseIf Boolean.TryParse(value, returnValue) Then
            result = returnValue
        Else
            returnValue = False
        End If
        Return returnValue
    End Function

#End Region

#Region " DECIMAL "

    ''' <summary> Parses a value to Decimal. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy"> A dummy that distinguishes this method. </param>
    ''' <param name="value"> The value. </param>
    ''' <returns> Value if the value is a valid number; otherwise, Default. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="dummy")>
    Public Shared Function Parse(ByVal dummy As Decimal, ByVal value As String) As Decimal
        If value Is Nothing Then
            Throw New ArgumentNullException(NameOf(value), "Query not executed")
        ElseIf String.IsNullOrWhiteSpace(value) Then
            Throw New ArgumentException("Query returned an empty string", "value")
        Else
            Return Decimal.Parse(value, Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent,
                                 Globalization.CultureInfo.InvariantCulture)
        End If
    End Function

    ''' <summary> Tries to parse a Decimal reading. </summary>
    ''' <param name="value">  The value. </param>
    ''' <param name="result"> [in,out] The result. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryParse(ByVal value As String, ByRef result As Decimal) As Boolean
        If String.IsNullOrWhiteSpace(value) Then
            Return False
        ElseIf Decimal.TryParse(value, Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent,
                                Globalization.CultureInfo.InvariantCulture, result) Then
            Return True
        Else
            Return False
        End If

    End Function

#End Region

#Region " DOUBLE "

    ''' <summary> Parses a value to Double. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy"> A dummy that distinguishes this method. </param>
    ''' <param name="value"> The value. </param>
    ''' <returns> Value if the value is a valid number; otherwise, Default. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="dummy")>
    Public Shared Function Parse(ByVal dummy As Double, ByVal value As String) As Double
        If value Is Nothing Then
            Throw New ArgumentNullException(NameOf(value), "Query not executed")
        ElseIf String.IsNullOrWhiteSpace(value) Then
            Throw New ArgumentException("Query returned an empty string", "value")
        Else
            Return Double.Parse(value, Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent,
                                 Globalization.CultureInfo.InvariantCulture)
        End If
    End Function

    ''' <summary> Tries to parse a Double reading. </summary>
    ''' <param name="value">  The value. </param>
    ''' <param name="result"> [in,out] The result. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryParse(ByVal value As String, ByRef result As Double) As Boolean
        If String.IsNullOrWhiteSpace(value) Then
            Return False
        ElseIf Double.TryParse(value, Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent,
                               Globalization.CultureInfo.InvariantCulture, result) Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#Region " INTEGER "

    ''' <summary> Parses a value to Integer. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy"> A dummy that distinguishes this method. </param>
    ''' <param name="value"> The value. </param>
    ''' <returns> Value if the value is a valid number; otherwise, Default. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="dummy")>
    Public Shared Function Parse(ByVal dummy As Integer, ByVal value As String) As Integer
        If value Is Nothing Then
            Throw New ArgumentNullException(NameOf(value), "Query not executed")
        ElseIf String.IsNullOrWhiteSpace(value) Then
            Throw New ArgumentException("Query returned an empty string", "value")
        Else
            Return Integer.Parse(value, Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent,
                                 Globalization.CultureInfo.InvariantCulture)
        End If
    End Function

    ''' <summary> Tries to parse a value to Integer. </summary>
    ''' <param name="value">  The value. </param>
    ''' <param name="result"> [in,out] The result. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function TryParse(ByVal value As String, ByRef result As Integer) As Boolean
        Dim inf As Double
        If String.IsNullOrWhiteSpace(value) Then
            Return False
            ' check if we have an infinity.
        ElseIf SessionBase.TryParse(value, inf) AndAlso (inf > Integer.MaxValue OrElse inf < Integer.MinValue) Then
            If inf > Integer.MaxValue Then
                result = Integer.MaxValue
            Else
                result = Integer.MinValue
            End If
            Return True
        ElseIf Integer.TryParse(value, Globalization.NumberStyles.Number Or Globalization.NumberStyles.AllowExponent,
                                Globalization.CultureInfo.InvariantCulture, result) Then
            Return True
        Else

            Return False
        End If
    End Function

#End Region

#End Region

#Region " QUERY AND PARSE "

#Region " BOOLEAN "

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Boolean return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Boolean, ByVal format As String, ByVal ParamArray args() As Object) As Boolean?
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(format, args))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Boolean return value. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef value As Boolean, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Me.MakeEmulatedReplyIfEmpty(value)
        Return SessionBase.TryParse(Me.QueryTrimEnd(format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Boolean return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Boolean, ByVal dataToWrite As String) As Boolean
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(dataToWrite))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Boolean return value. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef value As Boolean, ByVal dataToWrite As String) As Boolean
        Me.MakeEmulatedReplyIfEmpty(value)
        Return SessionBase.TryParse(Me.QueryTrimEnd(dataToWrite), value)
    End Function

#End Region

#Region " DECIMAL "

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Decimal return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Decimal, ByVal format As String, ByVal ParamArray args() As Object) As Decimal
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(format, args))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Decimal return value. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef value As Decimal, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Me.MakeEmulatedReplyIfEmpty(value)
        Return SessionBase.TryParse(Me.QueryTrimEnd(format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Decimal return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Decimal, ByVal dataToWrite As String) As Decimal
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(dataToWrite))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Decimal return value. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef value As Decimal, ByVal dataToWrite As String) As Boolean
        Me.MakeEmulatedReplyIfEmpty(value)
        Return SessionBase.TryParse(Me.QueryTrimEnd(dataToWrite), value)
    End Function

#End Region

#Region " DOUBLE "

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Double return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Double, ByVal format As String, ByVal ParamArray args() As Object) As Double
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(format, args))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Double return value. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef value As Double, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Me.MakeEmulatedReplyIfEmpty(value)
        Return SessionBase.TryParse(Me.QueryTrimEnd(format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Double return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Double, ByVal dataToWrite As String) As Double
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(dataToWrite))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Double return value. </summary>
    ''' <param name="result">      [in,out] The result. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef result As Double, ByVal dataToWrite As String) As Boolean
        Me.MakeEmulatedReplyIfEmpty(result)
        Return SessionBase.TryParse(Me.QueryTrimEnd(dataToWrite), result)
    End Function
#End Region

#Region " TIME SPAN "

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. </summary>
    ''' <param name="format">      The format for parsing the result. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value. </returns>
    ''' <remarks>see also: https://msdn.microsoft.com/en-us/library/ee372287.aspx#Other
    '''          </remarks> 
    Public Overloads Function Query(ByVal format As String, ByVal dataToWrite As String) As TimeSpan
        Me.MakeEmulatedReplyIfEmpty(TimeSpan.Zero)
        Return TimeSpan.ParseExact(Me.QueryTrimEnd(dataToWrite), format, Globalization.CultureInfo.InvariantCulture)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the time span return value. </summary>
    ''' <param name="result">      [in,out] The result. </param>
    ''' <param name="format">      The format for parsing the result. For example, "s\.FFFFFFF", convert the
    '''                            value to time span from seconds. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef result As TimeSpan, ByVal format As String, ByVal dataToWrite As String) As Boolean
        Me.MakeEmulatedReplyIfEmpty(result)
        Return TimeSpan.TryParseExact(Me.QueryTrimEnd(dataToWrite), format, Globalization.CultureInfo.InvariantCulture, result)
    End Function

#End Region

#Region " INTEGER "

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Integer return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Integer
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(format, args))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Integer return value. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef value As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Me.MakeEmulatedReplyIfEmpty(value)
        Return SessionBase.TryParse(Me.QueryTrimEnd(format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Integer return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Overloads Function Query(ByVal dummy As Integer, ByVal dataToWrite As String) As Integer
        Me.MakeEmulatedReplyIfEmpty(dummy)
        Return SessionBase.Parse(dummy, Me.QueryTrimEnd(dataToWrite))
    End Function

    ''' <summary> Performs a synchronous write of ASCII-encoded string data, followed by a synchronous
    ''' read. Parses the Integer return value. </summary>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQuery(ByRef value As Integer, ByVal dataToWrite As String) As Boolean
        Me.MakeEmulatedReplyIfEmpty(value)
        Return SessionBase.TryParse(Me.QueryTrimEnd(dataToWrite), value)
    End Function

#End Region

#Region " ENUM "

    ''' <summary> Parse enum value. </summary>
    ''' <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ''' type. </exception>
    ''' <param name="value"> The value. </param>
    ''' <returns> A Nullable(Of. </returns>
    Public Shared Function ParseEnumValue(Of T As Structure)(ByVal value As String) As Nullable(Of T)
        If String.IsNullOrWhiteSpace(value) Then
            Return New Nullable(Of T)
        Else
            Dim result As T = Nothing
            If [Enum].TryParse(value, result) Then
                Return result
            Else
                Throw New InvalidCastException(String.Format(Globalization.CultureInfo.CurrentCulture,
                                                             "Can't convert {0} to {1}", value, GetType(T).ToString()))
            End If
        End If
    End Function

    ''' <summary> Issues the query command and parses the returned values into an Enum using the enum value. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or none if unknown. </returns>
    Public Overloads Function QueryEnumValue(Of T As Structure)(ByVal value As Nullable(Of T), ByVal dataToWrite As String) As Nullable(Of T)
        Dim currentValue As String = value.ToString
        Me.MakeEmulatedReplyIfEmpty(currentValue)
        If Not String.IsNullOrWhiteSpace(dataToWrite) Then
            Me.WriteLine(dataToWrite)
            currentValue = Me.ReadLineTrimEnd()
        End If
        Return SessionBase.ParseEnumValue(Of T)(currentValue)
    End Function

    ''' <summary> Issues the query command and parses the returned values into an Enum using the enum name. </summary>
    ''' <param name="value">        The value. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or none if unknown. </returns>
    Public Overloads Function QueryEnum(Of T As Structure)(ByVal value As Nullable(Of T), ByVal dataToWrite As String) As Nullable(Of T)
        Dim currentValue As String = value.ToString
        Me.MakeEmulatedReplyIfEmpty(currentValue)
        If Not String.IsNullOrWhiteSpace(dataToWrite) Then
            Me.WriteLine(dataToWrite)
            currentValue = Me.ReadLineTrimEnd()
        End If
        If String.IsNullOrWhiteSpace(currentValue) Then
            Return New Nullable(Of T)
        Else
            Dim se As New StringEnumerator(Of T)
            Return se.ParseContained(currentValue.BuildDelimitedValue)
        End If
    End Function

    ''' <summary> Synchronously writes the Enum name without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format for creating the data to write. </param>
    ''' <returns> The value or none if unknown. </returns>
    Public Function Write(Of T As Structure)(ByVal value As T, ByVal commandFormat As String) As Nullable(Of T)
        If Not String.IsNullOrWhiteSpace(commandFormat) Then
            Dim se As New StringEnumerator(Of T)
            Me.WriteLine(commandFormat, se.ToString(value).ExtractBetween())
        End If
        Return value
    End Function

    ''' <summary> Synchronously writes the Enum value without reading back the value from the device. </summary>
    ''' <param name="value">         The value. </param>
    ''' <param name="commandFormat"> The command format for creating the data to write. </param>
    ''' <returns> The value or none if unknown. </returns>
    Public Shadows Function WriteEnumValue(Of T As Structure)(ByVal value As T, ByVal commandFormat As String) As Nullable(Of T)
        If Not String.IsNullOrWhiteSpace(commandFormat) Then
            Dim v As Integer = CInt([Enum].ToObject(GetType(T), value))
            Me.WriteLine(commandFormat, v)
        End If
        Return value
    End Function

#End Region

#End Region


End Class
