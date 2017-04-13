''' <summary> Defines a SCPI Source Channel Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public Class SourceChannelSubsystem
    Inherits VI.Scpi.SourceChannelSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceChannelSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Public Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(channelNumber, statusSubsystem)
        Me.SupportedFunctionModes = VI.SourceFunctionModes.Current Or
                                        VI.SourceFunctionModes.Voltage
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FunctionMode = SourceFunctionModes.Voltage
        Me.Level = 0.5 ' volt RMS
        Me.SupportedFunctionModes = SourceFunctionModes.Current Or SourceFunctionModes.Voltage
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " FUNCTION MODE "

    ''' <summary> Gets or sets the cached source Function Mode. </summary>
    ''' <value> The <see cref="SourceFunctionModes">source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overrides Property FunctionMode As SourceFunctionModes?
        Get
            Return MyBase.FunctionMode
        End Get
        Protected Set(ByVal value As SourceFunctionModes?)
            If Not Nullable.Equals(Me.FunctionMode, value) Then
                If value.HasValue Then
                    If value.Value = SourceFunctionModes.Voltage Then
                        Me.LevelRange = New Core.Pith.RangeR(0.005, 1, 0.001)
                    ElseIf value.Value = SourceFunctionModes.Current Then
                        Me.LevelRange = New Core.Pith.RangeR(0.0002, 0.02, 0.00002)
                    Else
                        Me.LevelRange = New Core.Pith.RangeR(0, 0)
                    End If
                Else
                    Me.LevelRange = New Core.Pith.RangeR(0, 0)
                End If
                MyBase.FunctionMode = value
            End If
        End Set
    End Property

    ''' <summary> Gets or sets the function mode query command. </summary>
    ''' <value> The function mode query command, e.g., :SOUR:FUNC? </value>
    Protected Overrides ReadOnly Property FunctionModeQueryCommand As String
        Get
            Return $":SOUR{Me.ChannelNumber}:MODE?"
        End Get
    End Property

    ''' <summary> Gets or sets the function mode command. </summary>
    ''' <value> The function mode command, e.g., :SOUR:FUNC {0}. </value>
    Protected Overrides ReadOnly Property FunctionModeCommandFormat As String
        Get
            Return $":SOUR{Me.ChannelNumber}:MODE {{0}}"
        End Get
    End Property

#End Region

#Region " LEVEL "

    ''' <summary> Gets the Level command format. </summary>
    ''' <value> The Level command format. </value>
    Protected Overrides ReadOnly Property LevelCommandFormat As String
        Get
            Return $":SOUR{Me.ChannelNumber}:{Me.FunctionCode}:LEV {{0}}"
        End Get
    End Property

    ''' <summary> Gets the Level query command. </summary>
    ''' <value> The Level query command. </value>
    Protected Overrides ReadOnly Property LevelQueryCommand As String
        Get
            Return $":SOUR{Me.ChannelNumber}:{Me.FunctionCode}:LEV?"
        End Get
    End Property

#End Region

#End Region

End Class
