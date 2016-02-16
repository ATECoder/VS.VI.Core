
Partial Public Class SessionBase

#Region " QUERY PRINT "

    Public Property PrintCommandFormat As String = "_G.print({0})"

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. </summary>
    ''' <param name="dataToPrint"> The LUA command to print. </param>
    ''' <returns> The received message without the <see cref="Termination">termination characters</see>. </returns>
    Public Function QueryPrintTrimEnd(ByVal dataToPrint As String) As String
        Return Me.QueryTrimEnd(Me.PrintCommandFormat, dataToPrint)
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. </summary>
    ''' <param name="format"> The format for building the LUA command to be printed. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The received message without the <see cref="Termination">termination characters</see>. </returns>
    Public Function QueryPrintTrimEnd(ByVal format As String, ByVal ParamArray args() As Object) As String
        Return Me.QueryTrimEnd(Me.PrintCommandFormat, String.Format(Globalization.CultureInfo.InvariantCulture, format, args))
    End Function

#End Region

#Region " QUERY PRINT AND PARSE "

#Region " BOOLEAN "

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Boolean return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="format">  The format for building the LUA command to be printed. </param>
    ''' <param name="args">    The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Boolean, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(1, format, args))
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Boolean return value. </summary>
    ''' <param name="format"> The format for building the LUA command to be printed. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef value As Boolean, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(1, format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a
    ''' synchronous read. Parses the Boolean return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="dataToPrint"> The data to print. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Boolean, ByVal dataToPrint As String) As Boolean
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(1, dataToPrint))
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Boolean return value. </summary>
    ''' <param name="dataToPrint"> The data to print. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef value As Boolean, ByVal dataToPrint As String) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(1, dataToPrint), value)
    End Function

#End Region

#Region " DOUBLE "

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Double return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="format">  The format for building the LUA command to be printed. </param>
    ''' <param name="args">    The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Double, ByVal format As String, ByVal ParamArray args() As Object) As Double
        Return SessionBase.Parse(dummy, Me.QueryPrintTrimEnd(format, args))
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Double return value. </summary>
    ''' <param name="format"> The format for building the LUA command to be printed. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef value As Double, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintTrimEnd(format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a
    ''' synchronous read. Parses the Double return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="dataToPrint"> The data to print. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Double, ByVal dataToPrint As String) As Double
        Return SessionBase.Parse(dummy, Me.QueryPrintTrimEnd(dataToPrint))
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Double return value. </summary>
    ''' <param name="dataToPrint"> The data to print. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef value As Double, ByVal dataToPrint As String) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintTrimEnd(dataToPrint), value)
    End Function

#End Region

#Region " INTEGER "

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Integer return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="format">  The format for building the LUA command to be printed. </param>
    ''' <param name="args">    The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Integer
        Return SessionBase.Parse(dummy, Me.QueryPrintTrimEnd(format, args))
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Integer return value. </summary>
    ''' <param name="format"> The format of the data to write. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef value As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintTrimEnd(format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a
    ''' synchronous read. Parses the Integer return value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="dataToPrint"> The data to print. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Integer, ByVal dataToPrint As String) As Integer
        Return SessionBase.Parse(dummy, Me.QueryPrintTrimEnd(dataToPrint))
    End Function

    ''' <summary> Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    ''' read. Parses the Integer return value. </summary>
    ''' <param name="dataToPrint"> The data to print. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef value As Integer, ByVal dataToPrint As String) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintTrimEnd(dataToPrint), value)
    End Function

#End Region

#End Region

#Region " QUERY PRINT STRING "

    ''' <summary> Gets or sets the query print string format command. </summary>
    ''' <value> The query print string format command. </value>
    Public Property PrintCommandStringFormat As String = "_G.print(string.format('{0}',{1}))"

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           The format conforms to the 'C' query command and returns the Boolean outcome. </summary>
    ''' <remarks> The format string follows the same rules as the printf family of standard C
    ''' functions. The only differences are that the options or modifiers *, l, L, n, p, and h are
    ''' not supported and that there is an extra option, q. The q option formats a string in a form
    ''' suitable to be safely read back by the Lua interpreter: the string is written between double
    ''' quotes, and all double quotes, newlines, embedded zeros, and backslashes in the string are
    ''' correctly escaped when written. For instance, the call string.format('%q', 'a string with
    ''' ''quotes'' and [BS]n new line') will produce the string: a string with [BS]''quotes[BS]'' and
    ''' [BS]new line The options c, d, E, e, f, g, G, i, o, u, X, and x all expect a number as
    ''' argument, whereas q and s expect a string. This function does not accept string values
    ''' containing embedded zeros. </remarks>
    ''' <param name="format"> The LUA format of the data top be printed. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> The received message without the <see cref="Termination">termination characters</see>. </returns>
    Public Function QueryPrintStringFormatTrimEnd(ByVal format As String, ByVal ParamArray args() As String) As String
        Return Me.QueryTrimEnd(Me.PrintCommandStringFormat, format, SessionBase.Parameterize(args))
    End Function

    ''' <summary> Gets or sets the print command string number format. </summary>
    ''' <value> The print command string number format. </value>
    Public Property PrintCommandStringNumberFormat As String = "_G.print(string.format('%{0}f',{1}))"

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           The format conforms to the 'C' query command and returns the Boolean outcome. </summary>
    ''' <param name="numberFormat"> Number of formats. </param>
    ''' <param name="dataToPrint">  The data to print. </param>
    ''' <returns> The received message without the <see cref="Termination">termination characters</see>. </returns>
    Public Function QueryPrintStringFormatTrimEnd(ByVal numberFormat As Decimal, ByVal dataToPrint As String) As String
        Return Me.QueryTrimEnd(Me.PrintCommandStringNumberFormat, numberFormat, dataToPrint)
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           The format conforms to the 'C' query command and returns the Boolean outcome. </summary>
    ''' <param name="numberFormat"> Number of formats. </param>
    ''' <param name="format">       The LUA format of the data top be printed. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> The received message without the <see cref="Termination">termination characters</see>. </returns>
    Public Function QueryPrintStringFormatTrimEnd(ByVal numberFormat As Decimal, ByVal format As String, ByVal ParamArray args() As Object) As String
        Return Me.QueryPrintStringFormatTrimEnd(numberFormat, String.Format(Globalization.CultureInfo.InvariantCulture, format, args))
    End Function

    ''' <summary> Gets or sets the query print string integer format command. </summary>
    ''' <value> The query print string integer format command. </value>
    Public Property PrintCommandStringIntegerFormat As String = "_G.print(string.format('%d',{1}))"

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           The format conforms to the 'C' query command and returns the Boolean outcome. </summary>
    ''' <param name="numberFormat"> The number format for integer type. </param>
    ''' <param name="dataToPrint">  The data to print. </param>
    ''' <returns> The received message without the <see cref="Termination">termination characters</see>. </returns>
    Public Function QueryPrintStringFormatTrimEnd(ByVal numberFormat As Integer, ByVal dataToPrint As String) As String
        Return Me.QueryTrimEnd(Me.PrintCommandStringIntegerFormat, numberFormat, dataToPrint)
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           The format conforms to the 'C' query command and returns the Boolean outcome. </summary>
    ''' <param name="numberFormat"> The number format for integer type. </param>
    ''' <param name="format">       The LUA format of the data top be printed. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> The received message without the <see cref="Termination">termination characters</see>. </returns>
    Public Function QueryPrintStringFormatTrimEnd(ByVal numberFormat As Integer, ByVal format As String, ByVal ParamArray args() As Object) As String
        Return Me.QueryPrintStringFormatTrimEnd(numberFormat, String.Format(Globalization.CultureInfo.InvariantCulture, format, args))
    End Function

    ''' <summary> Returns a string from the parameter array of arguments for use when running the
    ''' function. </summary>
    ''' <param name="args"> Specifies a parameter array of arguments. </param>
    Public Shared Function Parameterize(ByVal ParamArray args() As String) As String

        Dim arguments As New System.Text.StringBuilder
        Dim i As Integer
        If args IsNot Nothing AndAlso args.Length >= 0 Then
            For i = 0 To args.Length - 1
                If (i > 0) Then
                    arguments.Append(",")
                End If
                arguments.Append(args(i))
            Next i

        End If
        Return arguments.ToString

    End Function

#End Region

#Region " QUERY PRINT STRING AND PARSE "

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a
    ''' synchronous read. Parses the reading to Decimal. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">        A dummy that distinguishes this method. </param>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print
    ''' string format statement. </param>
    ''' <param name="dataToPrint">  The data to print. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Decimal, ByVal numberFormat As Decimal, ByVal dataToPrint As String) As Decimal
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(numberFormat, dataToPrint))
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to Decimal. </summary>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print string format statement. </param>
    ''' <param name="value">        [in,out] Value read from the instrument. </param>
    ''' <param name="dataToPrint">  The data to print. </param>
    ''' <returns> True returned value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryQueryPrint(ByVal numberFormat As Decimal, ByRef value As Decimal, ByVal dataToPrint As String) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(numberFormat, dataToPrint), value)
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to Decimal. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print string format statement. </param>
    ''' <param name="format">       The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Decimal, ByVal numberFormat As Decimal,
                                       ByVal format As String, ByVal ParamArray args() As Object) As Decimal
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(numberFormat, format, args))
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to Decimal. </summary>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print string format statement. </param>
    ''' <param name="value">        [in,out] Value read from the instrument. </param>
    ''' <param name="format">       The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> True returned value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryQueryPrint(ByVal numberFormat As Decimal, ByRef value As Decimal,
                                  ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(numberFormat, format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to Double. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">  A dummy that distinguishes this method. </param>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print string format statement. </param>
    ''' <param name="dataToPrint">  The data to print. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Double, ByVal numberFormat As Decimal, ByVal dataToPrint As String) As Double
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(numberFormat, dataToPrint))
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to Double. </summary>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print string format statement. </param>
    ''' <param name="value">        [in,out] Value read from the instrument. </param>
    ''' <param name="dataToPrint">  The data to print. </param>
    ''' <returns> True returned value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryQueryPrint(ByVal numberFormat As Decimal, ByRef value As Double, ByVal dataToPrint As String) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(numberFormat, dataToPrint), value)
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a
    ''' synchronous read. Parses the reading to Double. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">        A dummy that distinguishes this method. </param>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print
    ''' string format statement. </param>
    ''' <param name="format">       The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Double, ByVal numberFormat As Decimal,
                               ByVal format As String, ByVal ParamArray args() As Object) As Double
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(numberFormat, format, args))
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to Double. </summary>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print string format statement. </param>
    ''' <param name="value">        [in,out] Value read from the instrument. </param>
    ''' <param name="format">       The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> True returned value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryQueryPrint(ByVal numberFormat As Decimal, ByRef value As Double,
                                  ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(numberFormat, format, args), value)
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a
    ''' synchronous read. Parses the reading to Integer. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">        A dummy that distinguishes this method. </param>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print
    ''' string format statement. </param>
    ''' <param name="dataToPrint"> The data to print. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Integer, ByVal numberFormat As Integer, ByVal dataToPrint As String) As Integer
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(numberFormat, dataToPrint))
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to integer. </summary>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print
    ''' string format statement. </param>
    ''' <param name="value">        [in,out] Value read from the instrument. </param>
    ''' <param name="dataToPrint">  The data to print. </param>
    ''' <returns> True returned value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryQueryPrint(ByVal numberFormat As Integer, ByRef value As Integer, ByVal dataToPrint As String) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(numberFormat, dataToPrint), value)
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a
    ''' synchronous read. Parses the reading to Integer. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">        A dummy that distinguishes this method. </param>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print
    ''' string format statement. </param>
    ''' <param name="format">       The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Integer, ByVal numberFormat As Integer,
                               ByVal format As String, ByVal ParamArray args() As Object) As Integer
        Return SessionBase.Parse(dummy, Me.QueryPrintStringFormatTrimEnd(numberFormat, format, args))
    End Function

    ''' <summary> Performs a synchronous write of a Lua print string format command, followed by a synchronous read. 
    '''           Parses the reading to integer. </summary>
    ''' <param name="numberFormat"> The number format for the numeric value to use in the print
    ''' string format statement. </param>
    ''' <param name="value">        [in,out] Value read from the instrument. </param>
    ''' <param name="format">       The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> True returned value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Function TryQueryPrint(ByVal numberFormat As Integer, ByRef value As Integer,
                                  ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return SessionBase.TryParse(Me.QueryPrintStringFormatTrimEnd(numberFormat, format, args), value)
    End Function


#End Region

#Region " NIL "

    ''' <summary> Represents the LUA nil value. </summary>
    Public Const NilValue As String = "nil"

    ''' <summary> Returns <c>True</c> if the specified global exists. </summary>
    ''' <param name="value"> Specifies the global which to look for. </param>
    ''' <returns> <c>True</c> if the specified global exists; otherwise <c>False</c> </returns>
    Public Function IsGlobalExists(ByVal value As String) As Boolean
        Return Not String.Equals(SessionBase.NilValue, Me.QueryPrintTrimEnd(value))
    End Function

    ''' <summary> Returns <c>True</c> if the specified global is Nil. </summary>
    ''' <param name="value"> Specifies the global which to look for. </param>
    ''' <returns> <c>True</c> if the specified global exists; otherwise <c>False</c> </returns>
    Public Function IsNil(ByVal value As String) As Boolean
        Return String.Equals(SessionBase.NilValue, Me.QueryPrintTrimEnd(value))
    End Function

    ''' <summary> Returns <c>True</c> if the specified global is Nil. </summary>
    ''' <param name="format"> The format for building the global. </param>
    ''' <param name="args">   The format arguments. </param>
    ''' <returns> <c>True</c> if the specified global exists; otherwise <c>False</c> </returns>
    Public Function IsNil(ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Return String.Equals(SessionBase.NilValue, Me.QueryPrintTrimEnd(format, args))
    End Function

    ''' <summary> Checks the series of values and return <c>True</c> if any one of them is nil. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="values"> Specifies a list of nil objects to check. </param>
    ''' <returns> <c>True </c> if any value is nil; otherwise, <c>False</c> </returns>
    Public Function IsNil(ByVal ParamArray values() As String) As Boolean
        If values Is Nothing OrElse values.Length = 0 Then
            Throw New ArgumentNullException(NameOf(values))
        Else
            For Each value As String In values
                If Not String.IsNullOrWhiteSpace(value) Then
                    If Me.IsNil(value) Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False
    End Function

#End Region

#Region " TSP / LUA SYNTAX "

    ''' <summary> Returns true if the validation command returns true. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="format"> The format for constructing the assertion. </param>
    ''' <param name="args">   The format arguments. </param>
    Public Function IsStatementTrue(ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Dim value As Boolean = False
        Dim result As String = ""
        Try
            result = Me.QueryPrintTrimEnd(format, args)
            If String.Equals("true", result, StringComparison.OrdinalIgnoreCase) Then
                value = True
            ElseIf String.Equals("false", result, StringComparison.OrdinalIgnoreCase) Then
                value = False
            Else
                Throw New FormatException(String.Format("Statement '{0}' returned '{1}', which is not Boolean",
                                                    String.Format(format, args), result))
            End If
        Catch ex As FormatException
            Throw
        Catch ex As Exception
            Throw New OperationFailedException(String.Format("Statement '{0}' failed. Last message = '{1}'",
                                                             String.Format(format, args), Me.LastMessageSent))
        End Try
        Return value
    End Function

#End Region

#Region " NODE "

#Region " EXECUTE COMMAND - NODE "

    ''' <summary> Gets the execute command.  Requires node number and command arguments. </summary>
    Public Const ExecuteNodeCommandFormat As String = "_G.node[{0}].execute(""{1}"") _G.waitcomplete({0})"

    ''' <summary> Executes a command on the remote node. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="nodeNumber"> Specifies the node number. </param>
    ''' <param name="format">     The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> The command message. </returns>
    Public Function ExecuteCommand(ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object) As String
        If String.IsNullOrWhiteSpace(format) Then
            Throw New ArgumentNullException("format")
        Else
            Dim command As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
            Me.WriteLine(SessionBase.ExecuteNodeCommandFormat, nodeNumber, command)
            Return command
        End If
    End Function

#End Region

#Region " QUERY - NODE "

    ''' <summary> Gets the value returned by executing a command on the node.
    ''' Requires node number and value to get arguments. </summary>
    Public Const NodeValueGetterCommandFormat1 As String = "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete({0}) _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())"

    ''' <summary> Executes a query on the remote node and prints the result. This leaves an item in the
    ''' input buffer that must be retried. </summary>
    ''' <param name="nodeNumber">  Specifies the remote node number. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    Public Sub ExecuteQuery(ByVal nodeNumber As Integer, ByVal dataToWrite As String)
        If String.IsNullOrWhiteSpace(dataToWrite) Then
            Throw New ArgumentNullException("dataToWrite")
        Else
            Me.WriteLine(SessionBase.NodeValueGetterCommandFormat1, nodeNumber, dataToWrite)
        End If
    End Sub

    ''' <summary> Executes a query on the remote node prints the result. This leaves an item in the
    ''' input buffer that must be retried. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="format">     The format for constructing the data to write. </param>
    ''' <param name="args">       The format arguments. </param>
    Public Sub ExecuteQuery(ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object)
        If String.IsNullOrWhiteSpace(format) Then
            Throw New ArgumentNullException("format")
        Else
            Dim command As String = String.Format(Globalization.CultureInfo.CurrentCulture, format, args)
            Me.WriteLine(SessionBase.NodeValueGetterCommandFormat1, nodeNumber, command)
        End If
    End Sub

    ''' <summary> Executes a command on the remote node and prints the result, followed by a synchronous
    ''' read. </summary>
    ''' <param name="nodeNumber">  Specifies the remote node number. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The string. </returns>
    Public Function QueryPrintTrimEnd(ByVal nodeNumber As Integer, ByVal dataToWrite As String) As String
        Me.ExecuteQuery(nodeNumber, dataToWrite)
        Return Me.ReadLineTrimEnd()
    End Function

    ''' <summary> Executes a command on the remote node and prints the result, followed by a synchronous
    ''' read. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="format">     The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> The string. </returns>
    Public Function QueryPrintTrimEnd(ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object) As String
        Me.ExecuteQuery(nodeNumber, format, args)
        Return Me.ReadLineTrimEnd()
    End Function

#End Region

#Region " QUERY - NODE AND PARSE "

#Region " BOOLEAN "

    ''' <summary> Executes a command on the remote node and prints the result, followed by a
    ''' synchronous read. Parses the results as a Boolean. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">      A dummy that distinguishes this method. </param>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="format">     The format for constructing the data to write. </param>
    ''' <param name="args">       The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Boolean, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Boolean?
        Me.ExecuteQuery(nodeNumber, format, args)
        Return SessionBase.Parse(dummy, Me.ReadLineTrimEnd())
    End Function

    ''' <summary> Executes a command on the remote node and prints the result, followed by a synchronous
    ''' read. Parses the results as a Boolean. </summary>
    ''' <param name="result">     The result value. </param>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="format">     The format for constructing the data to write. </param>
    ''' <param name="args">         The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef result As Boolean, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Me.ExecuteQuery(nodeNumber, format, args)
        Return SessionBase.TryParse(Me.ReadLineTrimEnd(), result)
    End Function

    ''' <summary> Executes a command on the remote node and prints the result, followed by a
    ''' synchronous read. Parses the results as a Boolean. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="nodeNumber">  Specifies the remote node number. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Boolean, ByVal nodeNumber As Integer, ByVal dataToWrite As String) As Boolean
        Me.ExecuteQuery(nodeNumber, dataToWrite)
        Return SessionBase.Parse(dummy, Me.ReadLineTrimEnd())
    End Function

    ''' <summary> Executes a command on the remote node and prints the result, followed by a synchronous
    ''' read. Parses the results as a Boolean. </summary>
    ''' <param name="result">       The result value. </param>
    ''' <param name="nodeNumber">   Specifies the remote node number. </param>
    ''' <param name="dataToWrite">  The data to write. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef result As Boolean, ByVal nodeNumber As Integer, ByVal dataToWrite As String) As Boolean
        Me.ExecuteQuery(nodeNumber, dataToWrite)
        Return SessionBase.TryParse(Me.ReadLineTrimEnd(), result)
    End Function

#End Region

#Region " DOUBLE "

    ''' <summary> Executes a command on the remote node and prints the result, followed by a
    ''' synchronous read. Parses the results as a Double. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">      A dummy that distinguishes this method. </param>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="format">     The format for constructing the data to write. </param>
    ''' <param name="args">       The format arguments. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Double, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Double?
        Me.ExecuteQuery(nodeNumber, format, args)
        Return SessionBase.Parse(dummy, Me.ReadLineTrimEnd())
    End Function

    ''' <summary> Executes a command on the remote node and retrieves a Double. </summary>
    ''' <param name="result">     The result value. </param>
    ''' <param name="nodeNumber"> Specifies the remote node number. </param>
    ''' <param name="format">     The format for constructing the data to write. </param>
    ''' <param name="args">       The format arguments. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef result As Double, ByVal nodeNumber As Integer, ByVal format As String, ByVal ParamArray args() As Object) As Boolean
        Me.ExecuteQuery(nodeNumber, format, args)
        Return SessionBase.TryParse(Me.ReadLineTrimEnd(), result)
    End Function

    ''' <summary> Executes a command on the remote node and prints the result, followed by a
    ''' synchronous read. Parses the results as a Double. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <exception cref="FormatException"> Thrown when the format of the received message is
    ''' incorrect. </exception>
    ''' <param name="dummy">       A dummy that distinguishes this method. </param>
    ''' <param name="nodeNumber">  Specifies the remote node number. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> The parsed value or default. </returns>
    Public Function QueryPrint(ByVal dummy As Double, ByVal nodeNumber As Integer, ByVal dataToWrite As String) As Double
        Me.ExecuteQuery(nodeNumber, dataToWrite)
        Return SessionBase.Parse(dummy, Me.ReadLineTrimEnd())
    End Function

    ''' <summary> Executes a command on the remote node and retrieves a Double. </summary>
    ''' <param name="result">      The result value. </param>
    ''' <param name="nodeNumber">  Specifies the remote node number. </param>
    ''' <param name="dataToWrite"> The data to write. </param>
    ''' <returns> <c>True</c> if the parsed value is valid. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Public Function TryQueryPrint(ByRef result As Double, ByVal nodeNumber As Integer, ByVal dataToWrite As String) As Boolean
        Me.ExecuteQuery(nodeNumber, dataToWrite)
        Return SessionBase.TryParse(Me.ReadLineTrimEnd(), result)
    End Function

#End Region

#End Region

#Region " NIL "

    ''' <summary> Returns <c>True</c> if the specified node global is Nil. </summary>
    ''' <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    ''' <param name="value">      Specifies the global which to look for. </param>
    ''' <returns> <c>True</c> if the specified node global exists; otherwise <c>False</c> </returns>
    Public Function IsNil(ByVal nodeNumber As Integer, ByVal value As String) As Boolean
        Return String.Equals(SessionBase.NilValue, Me.QueryPrintTrimEnd(nodeNumber, value))
    End Function

    ''' <summary> Checks the series of values and return <c>True</c> if any one of them is nil. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    ''' <param name="values">     Specifies a list of nil objects to check. </param>
    ''' <returns> <c>True </c> if any value is nil; otherwise, <c>False</c> </returns>
    Public Function IsNil(ByVal nodeNumber As Integer, ByVal ParamArray values() As String) As Boolean
        If values Is Nothing OrElse values.Length = 0 Then Throw New ArgumentNullException(NameOf(values))
        Dim affirmative As Boolean = False
        For Each value As String In values
            If Not String.IsNullOrWhiteSpace(value) Then
                If Me.IsNil(nodeNumber, value) Then
                    affirmative = True
                    Exit For
                End If
            End If
        Next
        Return affirmative
    End Function

    ''' <summary>
    ''' Checks the series of values and return <c>True</c> if any one of them is nil.
    ''' </summary>
    ''' <remarks> David, 11/30/2015. </remarks>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="isControllerNode"> true if this object is controller node. </param>
    ''' <param name="nodeNumber">       Specifies the remote node number to validate. </param>
    ''' <param name="values">           Specifies a list of nil objects to check. </param>
    ''' <returns> <c>True </c> if any value is nil; otherwise, <c>False</c> </returns>
    Public Function IsNil(ByVal isControllerNode As Boolean, ByVal nodeNumber As Integer, ByVal ParamArray values() As String) As Boolean
        If values Is Nothing OrElse values.Length = 0 Then Throw New ArgumentNullException(NameOf(values))
        If isControllerNode Then
            Return Me.IsNil(values)
        Else
            Return Me.IsNil(nodeNumber, values)
        End If
    End Function

    ''' <summary> Loops until the name is found or timeout. </summary>
    ''' <param name="nodeNumber"> Specifies the node number. </param>
    ''' <param name="name">       Specifies the script name. </param>
    ''' <param name="timeout">    The timeout. </param>
    ''' <returns> <c>True </c> if nil; otherwise, <c>False</c> </returns>
    Public Function WaitNotNil(ByVal nodeNumber As Integer, ByVal name As String, ByVal timeout As TimeSpan) As Boolean

        ' verify that the script exists. 
        Dim endTime As DateTime = DateTime.Now.Add(timeout)
        Dim detected As Boolean = Not Me.IsNil(nodeNumber, name)
        Do Until detected OrElse DateTime.Now > endTime
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(10)
            detected = Not Me.IsNil(nodeNumber, name)
        Loop
        Return detected

    End Function

#End Region

#End Region


End Class
