

Partial Public Class Session

#Region " ATTRIBUTES "

#Region " NATIVE TERMINATION ENABLED "

    ''' <summary> Queries the Termination Enabled sentinel. Also sets the
    ''' <see cref="NativeTerminationEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    <Obsolete("Not implemented")>
    Public Overrides Function QueryNativeTerminationEnabled() As Boolean?
        Me.NativeTerminationEnabled = True
        Return Me.NativeTerminationEnabled
    End Function

    ''' <summary> Writes the Termination Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    <Obsolete("Not implemented")>
    Public Overrides Sub WriteNativeTerminationEnabled(ByVal value As Boolean)
        Me.NativeTerminationEnabled = True
    End Sub

#End Region

#Region " NATIVE TERMINATION CHARACTER "

    ''' <summary> Queries the native termination character. </summary>
    ''' <returns> The termination native character. </returns>
    <Obsolete("Not implemented")>
    Public Overrides Function QueryNativeTerminationCharacter() As Byte?
        Me.nativeTerminationCharacter = 10
        Return Me.nativeTerminationCharacter
    End Function

    ''' <summary> Writes the native termination character. </summary>
    ''' <param name="value"> The termination character value. </param>
    <Obsolete("Not implemented")>
    Public Overrides Sub WriteNativeTerminationCharacter(ByVal value As Byte)
        Me.nativeTerminationCharacter = 10
    End Sub

#End Region

#End Region


End Class
