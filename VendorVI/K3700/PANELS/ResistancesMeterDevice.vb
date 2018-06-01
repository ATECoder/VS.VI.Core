Imports isr.VI.ExceptionExtensions

''' <summary> A resistances meter device using the K3700 instrument. </summary>
''' <license>
''' (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="3/22/2018" by="David" revision=""> Created. </history>
Public Class ResistancesMeterDevice
    Inherits Device

#Region " CONSTRUCTORS "

    ''' <summary> Default constructor. </summary>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Public Sub New()
        MyBase.New
        Me._Resistors = New ChannelResistorCollection
        Me._MultimeterSenseChannel = 912
        Me._MultimeterSourceChannel = 921
        Me.SlotCapacity = 30
    End Sub

    ''' <summary> Validated the given value. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="device"> The device. </param>
    ''' <returns> A ResistancesMeterDevice. </returns>
    Public Overloads Shared Function Validated(ByVal device As ResistancesMeterDevice) As ResistancesMeterDevice
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        Return device
    End Function

    ''' <summary> Creates a new Me. </summary>
    ''' <returns> A Me. </returns>
    Public Overloads Shared Function Create() As ResistancesMeterDevice
        Dim device As ResistancesMeterDevice = Nothing
        Try
            device = New ResistancesMeterDevice
        Catch
            If device IsNot Nothing Then device.Dispose()
            device = Nothing
            Throw
        End Try
        Return device
    End Function

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

#Region " DEVICE "

    ''' <summary>
    ''' Allows the derived device to take actions after initialization is completed.
    ''' </summary>
    Protected Overrides Sub OnInitialized()
        MyBase.OnInitialized()
        If Me.IsDeviceOpen Then
            Dim e As New isr.Core.Pith.ActionEventArgs
            If Not Me.TryConfigureMeter(Me.MultimeterSubsystem.PowerLineCycles.GetValueOrDefault(1), e) Then
                Me.Talker.Publish(TraceEventType.Warning, My.MyLibrary.TraceEventId, $"Files configuring meter;. Details: {e.Details}")
            End If
        End If
        Me.EnableMeasurements()
        Me.SafeSendPropertyChanged(NameOf(ResistancesMeterDevice.Resistors))
    End Sub

#End Region

#Region " SUBSYSTEMS "

#Region " MUTLIMETER "

    Private _MeasurementEnabled As Boolean
    Public Property MeasurementEnabled As Boolean
        Get
            Return Me._MeasurementEnabled
        End Get
        Set(value As Boolean)
            If value <> Me.MeasurementEnabled Then
                Me._MeasurementEnabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Enables the measurements. </summary>
    Public Overridable Sub EnableMeasurements()
        Me.MeasurementEnabled = Me.IsDeviceOpen AndAlso
                                MultimeterFunctionMode.ResistanceFourWire = Me.MultimeterSubsystem.FunctionMode.GetValueOrDefault(MultimeterFunctionMode.None) AndAlso
                                Me.MultimeterSubsystem.PowerLineCycles.HasValue
    End Sub

    Private _PowerLineCycles As Double

    ''' <summary> Gets or sets the power line cycles. </summary>
    ''' <value> The power line cycles. </value>
    Public Overridable Property PowerLineCycles As Double
        Get
            Return Me._PowerLineCycles
        End Get
        Set(value As Double)
            If value <> Me.PowerLineCycles Then
                Me._PowerLineCycles = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Handles the Multimeter subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As MultimeterSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(K3700.MultimeterSubsystem.PowerLineCycles)
                If subsystem.PowerLineCycles.HasValue Then Me.PowerLineCycles = subsystem.PowerLineCycles.Value
                Me.EnableMeasurements()
            Case NameOf(K3700.MultimeterSubsystem.FunctionMode)
                Me.EnableMeasurements()
        End Select
    End Sub

#End Region

#Region " CHANNEL "

    Private _ClosedChannels As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property ClosedChannels As String
        Get
            Return Me._ClosedChannels
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.ClosedChannels, StringComparison.OrdinalIgnoreCase) Then
                Me._ClosedChannels = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Handles the Channel subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As ChannelSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(K3700.ChannelSubsystem.ClosedChannels)
                Me.ClosedChannels = subsystem.ClosedChannels
        End Select
    End Sub

#End Region

#Region " STATUS "

    Private _LastError As String
    ''' <summary> Gets or sets the last reading. </summary>
    ''' <value> The last reading. </value>
    Public Property LastError As String
        Get
            Return Me._LastError
        End Get
        Set(value As String)
            If Not String.Equals(value, Me.LastError, StringComparison.OrdinalIgnoreCase) Then
                Me._LastError = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property


    ''' <summary> Reports the last error. </summary>
    Protected Sub OnLastError(ByVal lastError As VI.DeviceError)
        If lastError IsNot Nothing Then
            Me.LastError = lastError.CompoundErrorMessage
        End If
    End Sub

    ''' <summary> Handle the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub HandlePropertyChange(ByVal subsystem As StatusSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        MyBase.HandlePropertyChange(subsystem, propertyName)
        Select Case propertyName
            Case NameOf(StatusSubsystemBase.DeviceErrorsReport)
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.LastDeviceError)
                Me.OnLastError(subsystem.LastDeviceError)
            Case NameOf(StatusSubsystemBase.ErrorAvailable)
        End Select
    End Sub

#End Region

#End Region

#Region " MEASURE "

    Private _MultimeterSenseChannel As Integer
    ''' <summary> Gets or sets a list of multimeter channels. </summary>
    ''' <value> A List of multimeter channels. </value>
    Public Property MultimeterSenseChannel As Integer
        Get
            Return Me._MultimeterSenseChannel
        End Get
        Set(value As Integer)
            If Not String.Equals(Me.MultimeterSenseChannel, value) Then
                Me._MultimeterSenseChannel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _MultimeterSourceChannel As Integer
    ''' <summary> Gets or sets a list of multimeter channels. </summary>
    ''' <value> A List of multimeter channels. </value>
    Public Property MultimeterSourceChannel As Integer
        Get
            Return Me._MultimeterSourceChannel
        End Get
        Set(value As Integer)
            If Not String.Equals(Me.MultimeterSourceChannel, value) Then
                Me._MultimeterSourceChannel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    Private _SlotCapacity As Integer
    ''' <summary> Gets or sets a list of multimeter channels. </summary>
    ''' <value> A List of multimeter channels. </value>
    Public Property SlotCapacity As Integer
        Get
            Return Me._SlotCapacity
        End Get
        Set(value As Integer)
            If Not Integer.Equals(Me.SlotCapacity, value) Then
                Me._SlotCapacity = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Gets the number of sets of <paramref name="setSize"/> size that fits in a slot. </summary>
    ''' <param name="setSize"> Number of elements in a set. </param>
    ''' <returns> An Integer. </returns>
    Public Function SlotSetCapacity(ByVal setSize As Integer) As Integer
        Return CInt(Math.Floor(Me.SlotCapacity / setSize))
    End Function

    ''' <summary> Slot net capacity. </summary>
    ''' <param name="setSize"> Number of elements in a set. </param>
    ''' <returns> An Integer. </returns>
    Public Function SlotNetCapacity(ByVal setSize As Integer) As Integer
        Return setSize * Me.SlotSetCapacity(setSize)
    End Function

    ''' <summary> Linear relay number. </summary>
    ''' <param name="setNumber">     The set number. </param>
    ''' <param name="ordinalNumber"> The ordinal number within the set. </param>
    ''' <param name="setSize">       Number of elements in a set. </param>
    ''' <returns> An Integer. </returns>
    Public Function LinearRelayNumber(ByVal setNumber As Integer, ByVal ordinalNumber As Integer, ByVal setSize As Integer) As Integer
        Return ordinalNumber + (setNumber - 1) * setSize
    End Function

    ''' <summary>
    ''' Get the slot number for the specified resistance set ordinal number and set size.
    ''' </summary>
    ''' <param name="setNumber">     The set number. </param>
    ''' <param name="ordinalNumber"> The ordinal number within the set. </param>
    ''' <param name="setSize">       Number of. </param>
    ''' <returns> An Integer. </returns>
    Public Function SlotNumber(ByVal setNumber As Integer, ByVal ordinalNumber As Integer, ByVal setSize As Integer) As Integer
        Return 1 + CInt(Math.Floor(Me.LinearRelayNumber(setNumber, ordinalNumber, setSize) / Me.SlotNetCapacity(setSize)))
    End Function

    ''' <summary> Relay number. </summary>
    ''' <param name="setNumber">     The set number. </param>
    ''' <param name="ordinalNumber"> The ordinal number within the set. </param>
    ''' <param name="setSize">       Number of elements in a set. </param>
    ''' <returns> An Integer. </returns>
    Public Function SenseRelayNumber(ByVal setNumber As Integer, ByVal ordinalNumber As Integer, ByVal setSize As Integer) As Integer
        Return Me.LinearRelayNumber(setNumber, ordinalNumber, setSize) Mod Me.SlotNetCapacity(setSize)
    End Function

    ''' <summary> Source relay number. </summary>
    ''' <param name="setNumber">     The set number. </param>
    ''' <param name="ordinalNumber"> The ordinal number within the set. </param>
    ''' <param name="setSize">       Number of elements in a set. </param>
    ''' <returns> An Integer. </returns>
    Public Function SourceRelayNumber(ByVal setNumber As Integer, ByVal ordinalNumber As Integer, ByVal setSize As Integer) As Integer
        Return Me.SlotCapacity + Me.SenseRelayNumber(setNumber, ordinalNumber, setSize)
    End Function

    ''' <summary> Builds channel list. </summary>
    ''' <param name="setNumber">     The set number. </param>
    ''' <param name="ordinalNumber"> The ordinal number within the set. </param>
    ''' <param name="setSize">       Number of elements in a set. </param>
    ''' <returns> A String. </returns>
    Public Function BuildChannelList(ByVal setNumber As Integer, ByVal ordinalNumber As Integer, ByVal setSize As Integer) As String
        Dim slotBaseNumber As Integer = 1000 * Me.SlotNumber(setNumber, ordinalNumber, setSize)
        Dim builder As New System.Text.StringBuilder
        Dim delimiter As String = ";"
        builder.Append($"{slotBaseNumber + Me.SenseRelayNumber(setNumber, ordinalNumber, setSize)}{delimiter}")
        Return builder.ToString
    End Function

    ''' <summary> Populates the given resistors. </summary>
    ''' <param name="prefix">    The prefix. </param>
    ''' <param name="setNumber"> The set number. </param>
    ''' <param name="setSize">   Number of elements in a set. </param>
    Public Sub Populate(ByVal prefix As String, ByVal setNumber As Integer, ByVal setSize As Integer)
        Me._Resistors.Clear()
        For ordinalNumber As Integer = 1 To setSize
            Me._Resistors.Add(New ChannelResistor($"{prefix}{ordinalNumber}", Me.BuildChannelList(setNumber, ordinalNumber, setSize)))
        Next
    End Sub

    ''' <summary> Populates the given resistors. </summary>
    ''' <param name="resistors"> The resistors. </param>
    Public Sub Populate(ByVal resistors As ChannelResistorCollection)
        Me._Resistors = resistors
    End Sub

    ''' <summary> Gets or sets the resistors. </summary>
    ''' <value> The resistors. </value>
    Public ReadOnly Property Resistors As ChannelResistorCollection

    ''' <summary> Configure meter. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="powerLineCycles"> The power line cycles. </param>
    Public Sub ConfigureMeter(ByVal powerLineCycles As Double)
        Dim activity As String = $"Checking {Me.Session.ResourceName} Is open"
        If Me.IsDeviceOpen Then
            activity = $"Configuring function mode {MultimeterFunctionMode.ResistanceFourWire}"
            Dim expectedMeasureFunction As MultimeterFunctionMode = MultimeterFunctionMode.ResistanceFourWire
            Dim measureFunction As MultimeterFunctionMode? = Me.MultimeterSubsystem.ApplyFunctionMode(expectedMeasureFunction)
            If measureFunction.HasValue Then
                If expectedMeasureFunction <> measureFunction.Value Then
                    Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedMeasureFunction } <> Actual {measureFunction.Value}")
                End If
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}--no value set")
            End If
            activity = $"Configuring power line cycles {powerLineCycles}"
            Dim actualPowerLineCycles As Double? = Me.MultimeterSubsystem.ApplyPowerLineCycles(powerLineCycles)
            If actualPowerLineCycles.HasValue Then
                If powerLineCycles <> actualPowerLineCycles.Value Then
                    Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {powerLineCycles} <> Actual {actualPowerLineCycles.Value}")
                End If
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}--no value set")
            End If
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device Is Not open")
        End If
    End Sub

    ''' <summary> Attempts to configure meter from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="powerLineCycles"> The power line cycles. </param>
    ''' <param name="e">               Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryConfigureMeter(ByVal powerLineCycles As Double, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"Configuring resistances meter {Me.Session.ResourceName}"
        Try
            Me.ConfigureMeter(powerLineCycles)
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure resistance. </summary>
    ''' <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    '''                                             null. </exception>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    ''' <param name="resistor"> The resistor. </param>
    Public Sub MeasureResistance(ByVal resistor As ChannelResistor)
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim activity As String = $"measuring {resistor.Title}"
        If Me.IsDeviceOpen Then
            activity = $"{resistor.Title} opening channels"
            Dim expectedChannelList As String = ""
            Dim actualChannelList As String = Me.ChannelSubsystem.ApplyOpenAll(TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            activity = $"{resistor.Title} closing {resistor.ChannelList}"
            expectedChannelList = resistor.ChannelList
            actualChannelList = Me.ChannelSubsystem.ApplyClosedChannels(expectedChannelList, TimeSpan.FromSeconds(2))
            If expectedChannelList <> actualChannelList Then
                Throw New VI.Pith.OperationFailedException($"Failed {activity} Expected {expectedChannelList } <> Actual {actualChannelList}")
            End If

            activity = $"measuring {resistor.Title}"
            If Me.MultimeterSubsystem.Measure.HasValue Then
                resistor.Resistance = Me.MultimeterSubsystem.MeasuredValue.GetValueOrDefault(-1)
            Else
                Throw New VI.Pith.OperationFailedException($"Failed {activity}; driver returned nothing")
            End If
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub

    ''' <summary> Attempts to measure resistance from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="resistor"> The resistor. </param>
    ''' <param name="e">        Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureResistance(ByVal resistor As ChannelResistor, ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        If resistor Is Nothing Then Throw New ArgumentNullException(NameOf(resistor))
        Dim activity As String = $"measuring {resistor.Title}"
        Try
            Me.MeasureResistance(resistor)
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

    ''' <summary> Measure resistors. </summary>
    ''' <exception cref="VI.Pith.OperationFailedException"> Thrown when operation failed to execute. </exception>
    Public Sub MeasureResistors()
        Dim activity As String = $"Measuring resistors at {Me.Session.ResourceName}"
        If Me.IsDeviceOpen Then
            For Each resistor As ChannelResistor In Me.Resistors
                activity = $"Measuring {resistor.Title} at {Me.Session.ResourceName}"
                Me.MeasureResistance(resistor)
            Next
            Me.SafeSendPropertyChanged(NameOf(ResistancesMeterDevice.Resistors))
        Else
            Throw New VI.Pith.OperationFailedException($"Failed {activity}; VISA session to this device is not open")
        End If
    End Sub

    ''' <summary> Attempts to measure resistors from the given data. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="e"> Cancel details event information. </param>
    ''' <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Function TryMeasureResistors(ByVal e As isr.Core.Pith.ActionEventArgs) As Boolean
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        Dim activity As String = $"Measuring resistors at {Me.Session.ResourceName}"
        Try
            Me.MeasureResistors()
        Catch ex As Exception
            e.RegisterCancellation(Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception {activity};. {ex.ToFullBlownString}"))
        End Try
        Return Not e.Cancel
    End Function

#End Region

#Region " TALKER "

    ''' <summary> Identify talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        MyBase.IdentifyTalkers()
        My.MyLibrary.Identify(Talker)
    End Sub

#End Region

End Class

