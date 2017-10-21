

Partial Public Class SessionBase

#Region " ATTRIBUTES "

#Region " NATIVE TERMINATION ENABLED "

    ''' <summary> Termination enabled. </summary>
    Private _NativeTerminationEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Termination is enabled. </summary>
    ''' <remarks>
    ''' VI_ATTR_TERMCHAR is the termination character. When the termination character is read and
    ''' VI_ATTR_TERMCHAR_EN is enabled during a read operation, the read operation terminates.
    ''' 
    ''' For a Serial INSTR session, VI_ATTR_TERMCHAR Is Read/Write when the corresponding session Is
    ''' Not enabled To receive VI_EVENT_ASRL_TERMCHAR events. When the session Is enabled To receive
    ''' VI_EVENT_ASRL_TERMCHAR events, the attribute VI_ATTR_TERMCHAR Is Read Only. For all other
    ''' session types, the attribute VI_ATTR_TERMCHAR Is always Read/Write.
    ''' </remarks>
    ''' <value>
    ''' <c>null</c> if state is not known; <c>True</c> if termination is enabled; otherwise,
    ''' <c>False</c>.
    ''' </value>
    Public Property NativeTerminationEnabled As Boolean?
        Get
            Return Me._NativeTerminationEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.NativeTerminationEnabled, value) Then
                Me._NativeTerminationEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Termination Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyNativeTerminationEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteNativeTerminationEnabled(value)
        Return Me.QueryNativeTerminationEnabled()
    End Function

    ''' <summary> Queries the Termination Enabled sentinel. Also sets the
    ''' <see cref="NativeTerminationEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public MustOverride Function QueryNativeTerminationEnabled() As Boolean?

    ''' <summary> Writes the Termination Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    Public MustOverride Sub WriteNativeTerminationEnabled(ByVal value As Boolean)

#End Region

#Region " NATIVE TERMINATION CHARACTER "

    Private _NativeTerminationCharacter As Byte?

    ''' <summary> Gets or sets a cached value of the native termination character. </summary>
    ''' <remarks>
    ''' VI_ATTR_TERMCHAR is the termination character. When the termination character is read and
    ''' VI_ATTR_TERMCHAR_EN is enabled during a read operation, the read operation terminates.
    ''' 
    ''' For a Serial INSTR session, VI_ATTR_TERMCHAR Is Read/Write when the corresponding session Is
    ''' Not enabled To receive VI_EVENT_ASRL_TERMCHAR events. When the session Is enabled To receive
    ''' VI_EVENT_ASRL_TERMCHAR events, the attribute VI_ATTR_TERMCHAR Is Read Only. For all other
    ''' session types, the attribute VI_ATTR_TERMCHAR Is always Read/Write.
    ''' </remarks>
    ''' <value> The native termination character. </value>
    Public Property nativeTerminationCharacter As Byte?
        Get
            Return Me._NativeTerminationCharacter
        End Get
        Protected Set(ByVal value As Byte?)
            If Not Byte?.Equals(Me.nativeTerminationCharacter, value) Then
                Me._NativeTerminationCharacter = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the native termination character. </summary>
    ''' <param name="value"> The termination character value. </param>
    Public Function ApplyNativeTerminationCharacter(ByVal value As Byte) As Byte?
        Me.WriteNativeTerminationCharacter(value)
        Return Me.QueryNativeTerminationCharacter()
    End Function

    ''' <summary> Queries the native termination character </summary>
    Public MustOverride Function QueryNativeTerminationCharacter() As Byte?

    ''' <summary> Writes the native termination character. </summary>
    ''' <param name="value"> The termination character value. </param>
    Public MustOverride Sub WriteNativeTerminationCharacter(ByVal value As Byte)

#End Region

#End Region

End Class
