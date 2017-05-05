Imports isr.Core.Pith
''' <summary> Defines the Calculate Channel SCPI subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public MustInherit Class ChannelTriggerSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me.ChannelNumber = channelNumber
    End Sub

#End Region

#Region " CHANNEL "

    ''' <summary> Gets or sets the channel number. </summary>
    ''' <value> The channel number. </value>
    Public ReadOnly Property ChannelNumber As Integer

#End Region

#Region " IMMEDIATE "

    ''' <summary> Gets or sets the initiate command. </summary>
    ''' <value> The initiate command. </value>
    ''' <remarks> SCPI: ":INIT&lt;c#&gt;:IMM" </remarks>
    Protected Overridable ReadOnly Property InitiateCommand As String

    ''' <summary> Changes the state of the channel to the initiation state of the trigger system. </summary>
    Public Sub Initiate()
        Me.Write(Me.InitiateCommand)
    End Sub

#End Region

#Region " Continuous ENABLED "

    Private _ContinuousEnabled As Boolean?
    ''' <summary> Gets or sets the cached Continuous Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Continuous Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property ContinuousEnabled As Boolean?
        Get
            Return Me._ContinuousEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ContinuousEnabled, value) Then
                Me._ContinuousEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Continuous Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyContinuousEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteContinuousEnabled(value)
        Return Me.QueryContinuousEnabled()
    End Function

    ''' <summary> Gets the continuous trigger enabled query command. </summary>
    ''' <value> The continuous trigger enabled query command. </value>
    ''' <remarks> SCPI: ":INIT&lt;c#&gt;:CONT?" </remarks>
    Protected Overridable ReadOnly Property ContinuousEnabledQueryCommand As String

    ''' <summary> Queries the Continuous Enabled sentinel. Also sets the
    ''' <see cref="ContinuousEnabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryContinuousEnabled() As Boolean?
        Me.ContinuousEnabled = Me.Query(Me.ContinuousEnabled, Me.ContinuousEnabledQueryCommand)
        Return Me.ContinuousEnabled
    End Function

    ''' <summary> Gets the continuous trigger enabled command Format. </summary>
    ''' <value> The continuous trigger enabled query command. </value>
    ''' <remarks> SCPI: ":INIT&lt;c#&gt;:CONT {0:1;1;0}" </remarks>
    Protected Overridable ReadOnly Property ContinuousEnabledCommandFormat As String

    ''' <summary> Writes the Continuous Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteContinuousEnabled(ByVal value As Boolean) As Boolean?
        Me.ContinuousEnabled = Me.Write(value, Me.ContinuousEnabledCommandFormat)
        Return Me.ContinuousEnabled
    End Function

#End Region

End Class

