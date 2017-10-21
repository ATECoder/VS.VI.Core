

Partial Public Class Session

#Region " ATTRIBUTES "

#Region " NATIVE TERMINATION ENABLED "

    ''' <summary> Queries the Termination Enabled sentinel. Also sets the
    ''' <see cref="NativeTerminationEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Overrides Function QueryNativeTerminationEnabled() As Boolean?
        Me.NativeTerminationEnabled = Me.ReadAttribute(Ivi.Visa.NativeVisaAttribute.TerminationCharacterEnabled, Me.NativeTerminationEnabled.GetValueOrDefault(False))
        Return Me.NativeTerminationEnabled
    End Function

    ''' <summary> Writes the Termination Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    Public Overrides Sub WriteNativeTerminationEnabled(ByVal value As Boolean)
        Me.WriteAttribute(Ivi.Visa.NativeVisaAttribute.TerminationCharacterEnabled, value)
        Me.NativeTerminationEnabled = value
    End Sub

#End Region

#Region " NATIVE TERMINATION CHARACTER "

    ''' <summary> Queries the native termination character. </summary>
    ''' <returns> The termination native character. </returns>
    Public Overrides Function QueryNativeTerminationCharacter() As Byte?
        Me.nativeTerminationCharacter = Me.ReadAttribute(Ivi.Visa.NativeVisaAttribute.TerminationCharacter,
                                                         Me.nativeTerminationCharacter.GetValueOrDefault(isr.Core.Pith.EscapeSequencesExtensions.NewLineValue))
        Return Me.nativeTerminationCharacter
    End Function

    ''' <summary> Writes the native termination character. </summary>
    ''' <param name="value"> The termination character value. </param>
    Public Overrides Sub WriteNativeTerminationCharacter(ByVal value As Byte)
        Me.WriteAttribute(Ivi.Visa.NativeVisaAttribute.TerminationCharacter, value)
        Me.nativeTerminationCharacter = value
    End Sub

#End Region

#End Region


End Class
