Partial Public Class ColdResistance

    ''' <summary> Validates the aperture described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateAperture(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ColdResistanceApertureMinimum AndAlso value <= .ColdResistanceApertureMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ColdResistanceApertureMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance aperture value of {0} is lower than the minimum of {1}.",
                                        value, .ColdResistanceApertureMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance aperture value of {0} is higher than the maximum of {1}.",
                                        value, .ColdResistanceApertureMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the current level described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateCurrentLevel(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ColdResistanceCurrentMinimum AndAlso value <= .ColdResistanceCurrentMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ColdResistanceCurrentMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance Current value of {0} is lower than the minimum of {1}.",
                                        value, .ColdResistanceCurrentMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance Current value of {0} is high than the maximum of {1}.",
                                        value, .ColdResistanceCurrentMaximum)
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
            Dim affirmative As Boolean = value >= .ColdResistanceMinimum AndAlso value <= .ColdResistanceMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ColdResistanceMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance Limit value of {0} is lower than the minimum of {1}.",
                                        value, .ColdResistanceMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance Limit value of {0} is high than the maximum of {1}.",
                                        value, .ColdResistanceMaximum)
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
            Dim affirmative As Boolean = value >= .ColdResistanceVoltageMinimum AndAlso value <= .ColdResistanceVoltageMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ColdResistanceVoltageMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance Voltage Limit value of {0} is lower than the minimum of {1}.",
                                        value, .ColdResistanceVoltageMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Cold Resistance Voltage Limit value of {0} is high than the maximum of {1}.",
                                        value, .ColdResistanceVoltageMaximum)
            End If
            Return affirmative
        End With
    End Function

End Class
