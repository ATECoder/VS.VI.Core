''' <summary> Defines the contract that must be implemented by a SCPI digital output Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class DigitalOutputBase
    Inherits VI.DigitalOutputBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="CompositeLimitBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.OutputMode = Scpi.OutputMode.EndTest
        Me.OutputSignalPolarity = Scpi.OutputSignalPolarity.Low
    End Sub

#End Region


#Region " OUTPUT MODE "

    ''' <summary> The Output Mode. </summary>
    Private _OutputMode As OutputMode?

    ''' <summary> Gets or sets the cached Output Mode. </summary>
    ''' <value> The <see cref="OutputMode">Output Mode</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property OutputMode As OutputMode?
        Get
            Return Me._OutputMode
        End Get
        Protected Set(ByVal value As OutputMode?)
            If Not Me.OutputMode.Equals(value) Then
                Me._OutputMode = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Output Mode. </summary>
    ''' <param name="value"> The Output Mode. </param>
    ''' <returns> The <see cref="OutputMode">source  Output Mode</see> or none if unknown. </returns>
    Public Function ApplyOutputMode(ByVal value As OutputMode) As OutputMode?
        Me.WriteOutputMode(value)
        Return Me.QueryOutputMode()
    End Function

    ''' <summary> Gets the Output Mode query command. </summary>
    ''' <value> The Output Mode query command. </value>
    ''' <remarks> SCPI: "SOUR2:MODE" </remarks>
    Protected Overridable ReadOnly Property OutputModeQueryCommand As String

    ''' <summary> Queries the Output Mode. </summary>
    ''' <returns> The <see cref="OutputMode"> Output Mode</see> or none if unknown. </returns>
    Public Function QueryOutputMode() As OutputMode?
        Me.OutputMode = Me.Query(Of OutputMode)(Me.OutputModeQueryCommand, Me.OutputMode)
        Return Me.OutputMode
    End Function

    ''' <summary> Gets the Output Mode command format. </summary>
    ''' <value> The Output Mode command format. </value>
    ''' <remarks> SCPI: "SOUR2:MODE" </remarks>
    Protected Overridable ReadOnly Property OutputModeCommandFormat As String

    ''' <summary> Writes the Output Mode without reading back the value from the device. </summary>
    ''' <param name="value"> The Output Mode. </param>
    ''' <returns> The <see cref="OutputMode"> Output Mode</see> or none if unknown. </returns>
    Public Function WriteOutputMode(ByVal value As OutputMode) As OutputMode?
        Me.OutputMode = Me.Write(Of OutputMode)(Me.OutputModeCommandFormat, value)
        Return Me.OutputMode
    End Function

#End Region

#Region " OUTPUT SIGNAL POLARITY EndTest/Busy "

    ''' <summary> The Output Polarity. </summary>
    Private _OutputSignalPolarity As OutputSignalPolarity?

    ''' <summary> Gets or sets the cached Output Polarity. </summary>
    ''' <value> The <see cref="OutputSignalPolarity">Output Polarity</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property OutputSignalPolarity As OutputSignalPolarity?
        Get
            Return Me._OutputSignalPolarity
        End Get
        Protected Set(ByVal value As OutputSignalPolarity?)
            If Not Me.OutputSignalPolarity.Equals(value) Then
                Me._OutputSignalPolarity = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Output Polarity. </summary>
    ''' <param name="value"> The Output Polarity. </param>
    ''' <returns> The <see cref="OutputSignalPolarity">source  Output Polarity</see> or none if unknown. </returns>
    Public Function ApplyOutputSignalPolarity(ByVal value As OutputSignalPolarity) As OutputSignalPolarity?
        Me.WriteOutputSignalPolarity(value)
        Return Me.QueryOutputSignalPolarity()
    End Function

    ''' <summary> Gets the Output Polarity query command. </summary>
    ''' <value> The Output Polarity query command. </value>
    ''' <remarks> SCPI: "SOUR2:Polarity" </remarks>
    Protected Overridable ReadOnly Property OutputSignalPolarityQueryCommand As String

    ''' <summary> Queries the Output Polarity. </summary>
    ''' <returns> The <see cref="OutputSignalPolarity"> Output Polarity</see> or none if unknown. </returns>
    Public Function QueryOutputSignalPolarity() As OutputSignalPolarity?
        Me.OutputSignalPolarity = Me.Query(Of OutputSignalPolarity)(Me.OutputSignalPolarityQueryCommand, Me.OutputSignalPolarity)
        Return Me.OutputSignalPolarity
    End Function

    ''' <summary> Gets the Output Polarity command format. </summary>
    ''' <value> The Output Polarity command format. </value>
    ''' <remarks> SCPI: "SOUR2:Polarity" </remarks>
    Protected Overridable ReadOnly Property OutputSignalPolarityCommandFormat As String

    ''' <summary> Writes the Output Polarity without reading back the value from the device. </summary>
    ''' <param name="value"> The Output Polarity. </param>
    ''' <returns> The <see cref="OutputSignalPolarity"> Output Polarity</see> or none if unknown. </returns>
    Public Function WriteOutputSignalPolarity(ByVal value As OutputSignalPolarity) As OutputSignalPolarity?
        Me.OutputSignalPolarity = Me.Write(Of OutputSignalPolarity)(Me.OutputSignalPolarityCommandFormat, value)
        Return Me.OutputSignalPolarity
    End Function

#End Region

End Class

''' <summary> Enumerates the digital output mode. </summary>
Public Enum OutputMode
    <ComponentModel.Description("Not Defined ()")> None
    ''' <summary> Using line 4 (2400) as end of test signal. </summary>
    <ComponentModel.Description("End of test (EOT)")> EndTest
    ''' <summary> Using line 4 (2400) as busy signal. </summary>
    <ComponentModel.Description("Busy (BUSY)")> Busy
End Enum

''' <summary> Enumerates the polarity of the end of test or busy signals. </summary>
Public Enum OutputSignalPolarity
    <ComponentModel.Description("Not Defined ()")> None
    <ComponentModel.Description("High (HI)")> High
    <ComponentModel.Description("Low (LO)")> Low
End Enum
