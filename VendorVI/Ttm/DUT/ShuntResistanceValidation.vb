Partial Public Class ShuntResistance

    ''' <summary> Validates the current level or range described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateCurrent(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ShuntResistanceCurrentMinimum AndAlso value <= .ShuntResistanceCurrentMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ShuntResistanceCurrentMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Shunt Resistance Current value of {0} is lower than the minimum of {1}.",
                                        value, .ShuntResistanceCurrentMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Shunt Resistance Current value of {0} is high than the maximum of {1}.",
                                        value, .ShuntResistanceCurrentMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the limit described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateLimit(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ShuntResistanceMinimum AndAlso value <= .ShuntResistanceMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ShuntResistanceMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Shunt Resistance Limit value of {0} is lower than the minimum of {1}.",
                                        value, .ShuntResistanceMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Shunt Resistance Limit value of {0} is high than the maximum of {1}.",
                                        value, .ShuntResistanceMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the voltage limit described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateVoltageLimit(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ShuntResistanceVoltageMinimum AndAlso value <= .ShuntResistanceVoltageMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ShuntResistanceVoltageMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Shunt Resistance Voltage Limit value of {0} is lower than the minimum of {1}.",
                                        value, .ShuntResistanceVoltageMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Shunt Resistance Voltage Limit value of {0} is high than the maximum of {1}.",
                                        value, .ShuntResistanceVoltageMaximum)
            End If
            Return affirmative
        End With
    End Function
End Class
