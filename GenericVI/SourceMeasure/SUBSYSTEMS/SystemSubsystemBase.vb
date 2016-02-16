''' <summary>
''' Defines a SCPI System Subsystem for a generic Source Measure instrument such as the Keithley 2400.
''' </summary>
''' <license>
''' (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
Public MustInherit Class SystemSubsystemBase
    Inherits VI.Scpi.SystemSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FourWireSenseEnabled = False
        Me.AutoZeroEnabled = True
    End Sub

#End Region

#Region " AUTO ZERO ENABLED "

    Private _AutoZeroEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether auto zero is enabled. </summary>
    ''' <value> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property AutoZeroEnabled() As Boolean?
        Get
            Return Me._AutoZeroEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoZeroEnabled, value) Then
                Me._AutoZeroEnabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.AutoZeroEnabled))
            End If
        End Set
    End Property

    ''' <summary> Queries the auto zero enabled state. </summary>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function QueryAutoZeroEnabled() As Boolean?

    ''' <summary> Writes and reads back the auto zero enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoZeroEnabled(value)
        Return Me.QueryAutoZeroEnabled()
    End Function

    ''' <summary> Writes the auto zero enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function WriteAutoZeroEnabled(ByVal value As Boolean) As Boolean?

#End Region

#Region " FOUR WIRE SENSE ENABLED "

    Private _FourWireSenseEnabled As Boolean?

    ''' <summary> Gets or sets a value indicating whether four wire sense is enabled. </summary>
    ''' <value> <c>True</c> if four wire sense mode is enabled; otherwise, <c>False</c>. </value>
    Public Property FourWireSenseEnabled() As Boolean?
        Get
            Return Me._FourWireSenseEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FourWireSenseEnabled, value) Then
                Me._FourWireSenseEnabled = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.FourWireSenseEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Four Wire Sense enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyFourWireSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteFourWireSenseEnabled(value)
        Return Me.QueryWireSenseEnabled()
    End Function

    ''' <summary> Queries the Four Wire Sense enabled state. </summary>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function QueryWireSenseEnabled() As Boolean?

    ''' <summary> Writes the Four Wire Sense enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function WriteFourWireSenseEnabled(ByVal value As Boolean) As Boolean?

#End Region

End Class
