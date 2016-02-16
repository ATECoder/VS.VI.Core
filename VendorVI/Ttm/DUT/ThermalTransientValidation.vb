Partial Public Class ThermalTransient

    ''' <summary> Validates the allowed voltage change described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateVoltageChange(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ThermalTransientVoltageMinimum AndAlso value <= .ThermalTransientVoltageMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientVoltageMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Voltage Change value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientVoltageMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Voltage Change value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalTransientVoltageMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the aperture described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateAperture(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ThermalTransientApertureMinimum AndAlso value <= .ThermalTransientApertureMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientApertureMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient aperture value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientApertureMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient aperture value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalTransientApertureMaximum)
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
            Dim affirmative As Boolean = value >= .ThermalTransientCurrentMinimum AndAlso value <= .ThermalTransientCurrentMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientCurrentMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient CurrentLevel value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientCurrentMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Current Level value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalTransientCurrentMaximum)
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
            Dim affirmative As Boolean = value >= .ThermalTransientVoltageChangeMinimum AndAlso value <= .ThermalTransientVoltageChangeMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientVoltageMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Limit value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientVoltageMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Limit value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalTransientVoltageMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the median filter size described by value. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateMedianFilterSize(value As Integer, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ThermalTransientMedianFilterLengthMinimum AndAlso
                value <= .ThermalTransientMedianFilterLengthMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientTracePointsMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Median Filter Length of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientMedianFilterLengthMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Median Filter Length value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalTransientMedianFilterLengthMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the Post Transient Delay described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidatePostTransientDelay(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .PostTransientDelayMinimum AndAlso value <= .PostTransientDelayMaximum
            If affirmative Then
                details = ""
            ElseIf value < .PostTransientDelayMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Post Transient Delay value of {0} is lower than the minimum of {1}.",
                                        value, .PostTransientDelayMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Post Transient Delay value of {0} is high than the maximum of {1}.",
                                        value, .PostTransientDelayMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the Duration described by value. </summary>
    ''' <param name="samplingInterval"> The sampling interval. </param>
    ''' <param name="tracePoints">      The trace points. </param>
    ''' <param name="details">          [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#")>
    Public Shared Function ValidatePulseWidth(ByVal samplingInterval As Double, ByVal tracePoints As Integer, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim pulseWidth As Double = samplingInterval * tracePoints
            Dim affirmative As Boolean = ThermalTransient.ValidateSamplingInterval(samplingInterval, details) AndAlso
                ThermalTransient.ValidateTracePoints(tracePoints, details) AndAlso
                pulseWidth >= .ThermalTransientDurationMinimum AndAlso pulseWidth <= .ThermalTransientDurationMaximum
            If affirmative Then
                details = ""
            ElseIf Not String.IsNullOrWhiteSpace(details) Then
                ' we have details. nothing to do.
            ElseIf pulseWidth < .ThermalTransientDurationMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Pulse Width of {0}s is lower than the minimum of {1}s.",
                                        pulseWidth, .ThermalTransientDurationMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Pulse Width of {0}s is higher than the maximum of {1}s.",
                                        pulseWidth, .ThermalTransientDurationMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the sampling interval described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateSamplingInterval(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ThermalTransientSamplingIntervalMinimum AndAlso value <= .ThermalTransientSamplingIntervalMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientSamplingIntervalMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Sample Interval value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientSamplingIntervalMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Sample Interval value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalTransientSamplingIntervalMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the trace points described by value. </summary>
    ''' <param name="value"> The value. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateTracePoints(value As Integer, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ThermalTransientTracePointsMinimum AndAlso value <= .ThermalTransientTracePointsMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientTracePointsMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Trace Points value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientTracePointsMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Trace Points value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalTransientTracePointsMaximum)
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
            details = ""
            Dim affirmative As Boolean = value >= .ThermalTransientVoltageMinimum AndAlso value <= .ThermalTransientVoltageMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalTransientVoltageMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Voltage Limit value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalTransientVoltageMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Transient Voltage Limit value of {0} is high than the maximum of {1}.",
                                        value, .ThermalTransientVoltageMaximum)
            End If
            Return affirmative
        End With
    End Function

    ''' <summary> Validates the transient voltage limit described by details. </summary>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if the voltage limit is in rage; <c>False</c> is the limit is too low. </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId:="$W0")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="4#")>
    Public Shared Function ValidateVoltageLimit(ByVal value As Double, ByVal maximumColdResistance As Double,
                                                ByVal currentLevel As Double, ByVal allowedVoltageChange As Double,
                                                ByRef details As String) As Boolean

        With My.MySettings.Default
            Dim expectedMaximumVoltage As Double = maximumColdResistance * currentLevel + allowedVoltageChange
            details = ""
            Dim affirmative As Boolean = ThermalTransient.ValidateVoltageChange(allowedVoltageChange, details) AndAlso
                ThermalTransient.ValidateCurrentLevel(currentLevel, details) AndAlso
                ThermalTransient.ValidateVoltageLimit(value, details) AndAlso value >= expectedMaximumVoltage
            If affirmative OrElse Not String.IsNullOrWhiteSpace(details) Then
                ' we have details. nothing to do.
            ElseIf expectedMaximumVoltage < expectedMaximumVoltage Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
"A Thermal transient voltage limit of {0} volts is too low;. Based on the cold resistance high limit and thermal transient current level and voltage change, the voltage might reach {1} volts.",
                                        value, expectedMaximumVoltage)
            End If
            Return affirmative
        End With

    End Function

#Region " ESTIMATOR "

    ''' <summary> Validates the ThermalCoefficient described by value. </summary>
    ''' <param name="value">   The value. </param>
    ''' <param name="details"> [in,out] The details. </param>
    ''' <returns> <c>True</c> if value is in range; otherwise, <c>False</c>. </returns>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    Public Shared Function ValidateThermalCoefficient(ByVal value As Double, ByRef details As String) As Boolean
        With My.MySettings.Default
            Dim affirmative As Boolean = value >= .ThermalCoefficientMinimum AndAlso value <= .ThermalCoefficientMaximum
            If affirmative Then
                details = ""
            ElseIf value < .ThermalCoefficientMinimum Then
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Coefficient value of {0} is lower than the minimum of {1}.",
                                        value, .ThermalCoefficientMinimum)
            Else
                details = String.Format(Globalization.CultureInfo.CurrentCulture,
                                        "Thermal Coefficient value of {0} is higher than the maximum of {1}.",
                                        value, .ThermalCoefficientMaximum)
            End If
            Return affirmative
        End With
    End Function

#End Region

End Class
