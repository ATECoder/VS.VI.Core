''' <summary> Defines a SCPI Compensation Channel Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="7/6/2016" by="David" revision="4.0.6031"> Created. </history>
Public Class CompensateChannelSubsystem
    Inherits VI.Scpi.CompensateChannelSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="SourceChannelSubsystem" /> class.
    ''' </summary>
    ''' <remarks> David, 7/6/2016. </remarks>
    ''' <param name="channelNumber">   A reference to a <see cref="VI.StatusSubsystemBase">message
    '''                                based session</see>. </param>
    ''' <param name="statusSubsystem"> The status subsystem. </param>
    Public Sub New(ByVal compensationType As CompensationTypes, ByVal channelNumber As Integer, ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(compensationType, channelNumber, statusSubsystem)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.Enabled = False
        If Me.CompensationType = VI.CompensationTypes.Load Then
            Me.ModelResistance = 50
            Me.ModelInductance = 0
            Me.ModelCapacitance = 0
        ElseIf Me.CompensationType = VI.CompensationTypes.OpenCircuit Then
            Me.ModelInductance = 0
            Me.ModelConductance = 0
        ElseIf Me.CompensationType = VI.CompensationTypes.ShortCircuit Then
            Me.ModelResistance = 0
            Me.ModelCapacitance = 0
        End If
    End Sub

#End Region

#Region " COMMAND SYNTAX "

#Region " COMMANDS "

    ''' <summary> Gets the Acquire measurements command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:COLL:ACQ:&lt;type&gt;". </remarks>
    ''' <value> The clear measurements command. </value>
    Protected Overrides ReadOnly Property AcquireMeasurementsCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:COLL:ACQ:{Me.CompensationTypeCode}"
        End Get
    End Property

    ''' <summary> Gets the clear measured data command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:&lt;type&gt;:COLL:CLE" </remarks>
    ''' <value> The clear measurements command. </value>
    Protected Overrides ReadOnly Property ClearMeasurementsCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2{Me.CompensationTypeCode}:COLL:CLE"
        End Get
    End Property

#End Region

#Region " ENABLED "

    ''' <summary> Gets the compensation enabled query command. </summary>
    ''' <value> The compensation enabled query command. </value>
    Protected Overrides ReadOnly Property EnabledQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:{Me.CompensationTypeCode}:STAT?"
        End Get
    End Property

    ''' <summary> Gets the compensation enabled command Format. </summary>
    ''' <value> The compensation enabled query command. </value>
    Protected Overrides ReadOnly Property EnabledCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:{Me.CompensationTypeCode}:STAT {0:1;1;0}"
        End Get
    End Property

#End Region

#Region " FREQUENCY STIMULUS POINTS "

    ''' <summary> Gets the Frequency Stimulus Points query command. </summary>
    ''' <remarks> SCPI: ":SENS&lt;ch#&gt;:CORR2:ZME:&lt;type&gt;:POIN?". </remarks>
    ''' <value> The Frequency Stimulus Points query command. </value>
    Protected Overrides ReadOnly Property FrequencyStimulusPointsQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:ZME:{Me.CompensationTypeCode}:POIN?"
        End Get
    End Property

#End Region

#Region " FREQUENCY ARRAY "

    ''' <summary> Gets the Frequency Array query command. </summary>
    ''' <value> The Frequency Array query command. </value>
    Protected Overrides ReadOnly Property FrequencyArrayQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:ZME:{Me.CompensationTypeCode}:FREQ?"
        End Get
    End Property


#End Region

#Region " IMPEDANCE ARRAY "

    ''' <summary> Gets the Impedance Array query command. </summary>
    ''' <value> The Impedance Array query command. </value>
    Protected Overrides ReadOnly Property ImpedanceArrayQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:ZME:{Me.CompensationTypeCode}:DATA?"
        End Get
    End Property

    ''' <summary> Gets the Impedance Array command Format. </summary>
    ''' <value> The Impedance Array query command. </value>
    Protected Overrides ReadOnly Property ImpedanceArrayCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:ZME:{Me.CompensationTypeCode}:DATA {0}"
        End Get
    End Property

#End Region

#Region " MODEL RESISTANCE "

    ''' <summary> Gets the Model Resistance query command. </summary>
    ''' <value> The Model Resistance query command. </value>
    Protected Overrides ReadOnly Property ModelResistanceQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:R?"
        End Get
    End Property

    ''' <summary> Gets the Model Resistance command Format. </summary>
    ''' <value> The Model Resistance query command. </value>
    Protected Overrides ReadOnly Property ModelResistanceCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:R {0}"
        End Get
    End Property

#End Region

#Region " MODEL CONDUCTANCE "

    ''' <summary> Gets the Model Conductance query command. </summary>
    ''' <value> The Model Conductance query command. </value>
    Protected Overrides ReadOnly Property ModelConductanceQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:G?"
        End Get
    End Property

    ''' <summary> Gets the Model Conductance command Format. </summary>
    ''' <value> The Model Conductance query command. </value>
    Protected Overrides ReadOnly Property ModelConductanceCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:G {0}"
        End Get
    End Property

#End Region

#Region " MODEL CAPACITANCE "

    ''' <summary> Gets the Model Capacitance query command. </summary>
    ''' <value> The Model Capacitance query command. </value>
    Protected Overrides ReadOnly Property ModelCapacitanceQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:C?"
        End Get
    End Property

    ''' <summary> Gets the Model Capacitance command Format. </summary>
    ''' <value> The Model Capacitance query command. </value>
    Protected Overrides ReadOnly Property ModelCapacitanceCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:C {0}"
        End Get
    End Property

#End Region

#Region " MODEL INDUCTANCE "

    ''' <summary> Gets the Model Inductance query command. </summary>
    ''' <value> The Model Inductance query command. </value>
    Protected Overrides ReadOnly Property ModelInductanceQueryCommand As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:L?"
        End Get
    End Property

    ''' <summary> Gets the Model Inductance command Format. </summary>
    ''' <value> The Model Inductance query command. </value>
    Protected Overrides ReadOnly Property ModelInductanceCommandFormat As String
        Get
            Return $":SENS{Me.ChannelNumber}:CORR2:CKIT:{Me.CompensationTypeCode}:L {0}"
        End Get
    End Property

#End Region

#End Region

End Class
