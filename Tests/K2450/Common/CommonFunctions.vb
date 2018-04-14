Partial Friend NotInheritable Class TestInfo

#Region " DEVICE OPEN, CLOSE, CHECK SUSBSYSTEMS "

    ''' <summary> Opens a session. </summary>
    ''' <param name="device"> The device. </param>
    Public Shared Sub OpenSession(ByVal device As VI.DeviceBase)
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        If Not TestInfo.ResourceLocated Then Assert.Inconclusive($"{TestInfo.ResourceTitle} not found")
        Dim actualCommand As String = device.Session.IsAliveCommand
        Dim expectedCommand As String = TestInfo.KeepAliveCommand
        Assert.AreEqual(expectedCommand, actualCommand, $"Keep alive command.")

        actualCommand = device.Session.IsAliveQueryCommand
        expectedCommand = TestInfo.KeepAliveQueryCommand
        Assert.AreEqual(expectedCommand, actualCommand, $"Keep alive query command.")

        Dim expectedErrorAvailableBits As Integer = VI.Pith.ServiceRequests.ErrorAvailable
        Dim actualErrorAvailableBits As Integer = device.Session.ErrorAvailableBits
        Assert.AreEqual(expectedErrorAvailableBits, actualErrorAvailableBits, $"Error available bits on creating device.")

        ' device.Session.ResourceTitle = TestInfo.ResourceTitle
        Dim e As New isr.Core.Pith.ActionEventArgs
        Dim actualBoolean As Boolean = device.TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
        Assert.IsTrue(actualBoolean, $"Failed to open session: {e.Details}")

        TestInfo.AssertMessageQueue()

    End Sub

    ''' <summary> Closes a session. </summary>
    ''' <param name="device"> The device. </param>
    Public Shared Sub CloseSession(ByVal device As VI.DeviceBase)
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        device.Session.Clear()
        device.CloseSession()
        Assert.IsFalse(device.IsDeviceOpen, $"Failed closing session to {device.ResourceName}")
        TestInfo.AssertMessageQueue()
    End Sub

    ''' <summary> Check line frequency. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="subsystem"> The subsystem. </param>
    Public Shared Sub CheckLineFrequency(ByVal subsystem As VI.StatusSubsystemBase)
        If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
        Dim actualFrequency As Double = subsystem.LineFrequency.GetValueOrDefault(0)
        Assert.AreEqual(TestInfo.LineFrequency, actualFrequency, $"{NameOf(StatusSubsystem.LineFrequency)} is {actualFrequency}; expected {TestInfo.LineFrequency}")
    End Sub

    ''' <summary> Check integration period. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="subsystem"> The subsystem. </param>
    Public Shared Sub CheckIntegrationPeriod(ByVal subsystem As VI.StatusSubsystemBase)
        If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
        Dim expectedPowerLineCycles As Double = TestInfo.InitialPowerLineCycles
        Dim expectedIntegrationPeriod As TimeSpan = StatusSubsystemBase.FromSecondsPrecise(expectedPowerLineCycles / TestInfo.LineFrequency)
        Dim actualIntegrationPeriod As TimeSpan = StatusSubsystemBase.FromPowerLineCycles(expectedPowerLineCycles)
        Assert.AreEqual(expectedIntegrationPeriod, actualIntegrationPeriod,
                                $"Integration period for {expectedPowerLineCycles} power line cycles is {actualIntegrationPeriod}; expected {expectedIntegrationPeriod}")

        Dim actualPowerLineCycles As Double = StatusSubsystemBase.ToPowerLineCycles(actualIntegrationPeriod)
        Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, TestInfo.LineFrequency / TimeSpan.TicksPerSecond,
                                $"Power line cycles is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")
    End Sub

    ''' <summary> Check model. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    Public Shared Sub CheckModel(ByVal subsystem As VI.StatusSubsystemBase)
        If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
        Assert.AreEqual(TestInfo.ResourceModel, subsystem.VersionInfo.Model, $"Version Info Model {subsystem.ResourceNameCaption}", Globalization.CultureInfo.CurrentCulture)
    End Sub

    ''' <summary> Opens close session. </summary>
    Public Shared Sub CheckDeviceErrors(ByVal subsystem As VI.StatusSubsystemBase)
        If Not TestInfo.ResourceLocated Then Assert.Inconclusive($"{TestInfo.ResourceTitle} not found")
        If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
        Dim e As New isr.Core.Pith.ActionEventArgs
        subsystem.TrySafeQueryDeviceErrors(e)
        Assert.IsFalse(e.Cancel, $"{subsystem.ResourceNameCaption}  failed reading existing errors {e.Details}")
        Assert.IsFalse(subsystem.ErrorAvailable, $"{subsystem.ResourceNameCaption} error available bit {subsystem.Session.ServiceRequestStatus:X} is on; last device error: {subsystem.LastDeviceError}")
        Assert.IsTrue(String.IsNullOrWhiteSpace(subsystem.DeviceErrorsReport), $"{subsystem.ResourceNameCaption} device errors: {subsystem.DeviceErrorsReport}")
        Assert.IsFalse(subsystem.LastDeviceError.IsError, $"{subsystem.ResourceNameCaption} last device error: {subsystem.LastDeviceError}")
    End Sub

    ''' <summary> Opens close session. </summary>
    Public Shared Sub ClearSessionCheckDeviceErrors(ByVal device As VI.DeviceBase)
        If Not TestInfo.ResourceLocated Then Assert.Inconclusive($"{TestInfo.ResourceTitle} not found")
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        device.Session.Clear()
        TestInfo.CheckDeviceErrors(device.StatusSubsystemBase)
        TestInfo.AssertMessageQueue()
    End Sub

    ''' <summary> Check termination. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="session"> The session. </param>
    Public Shared Sub CheckTermination(ByVal session As VI.Pith.SessionBase)
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
        Dim actualReadTerminationEnabled As Boolean = session.TerminationCharacterEnabled
        Dim expectedReadTerminationEnabled As Boolean = TestInfo.ReadTerminationEnabled
        Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Initial read termination enabled")

        expectedReadTerminationEnabled = TestInfo.ReadTerminationEnabled
        session.TerminationCharacterEnabled = expectedReadTerminationEnabled
        actualReadTerminationEnabled = session.TerminationCharacterEnabled
        Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Requested read termination")

        Dim actualTermination As Integer = session.TerminationCharacter
        Dim expectedTermination As Integer = TestInfo.TerminationCharacter
        Assert.AreEqual(expectedTermination, actualTermination, $"Termination character value")
        Assert.AreEqual(CByte(AscW(session.Termination(0))), session.TerminationCharacter, $"First termination character value")
    End Sub

    ''' <summary> Check channel subsystem information. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="subsystem"> The subsystem. </param>
    Public Shared Sub CheckChannelSubsystemInfo(ByVal subsystem As ChannelSubsystemBase)
        If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
        Assert.IsTrue(String.IsNullOrWhiteSpace(subsystem.ClosedChannels), $"Scan list {subsystem.ClosedChannels}; expected empty")
    End Sub

    ''' <summary> Check reading device errors. </summary>
    ''' <param name="device"> The device. </param>
    Public Shared Sub CheckReadingDeviceErrors(ByVal device As VI.DeviceBase)
        If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
        ' send an erroneous command
        Dim erroneousCommand As String = "*CLL"
        device.StatusSubsystemBase.Write(erroneousCommand)
        ' red the service request status; this should generate an error available 
        device.StatusSubsystemBase.ReadServiceRequestStatus()
        ' check the error available status
        Dim actualServiceRequest As VI.Pith.ServiceRequests = device.StatusSubsystemBase.ErrorAvailableBits
        Dim expectedSeriveRequest As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.ErrorAvailable
        Assert.AreEqual(expectedSeriveRequest, actualServiceRequest, $"Error bits expected {expectedSeriveRequest:X} <> actual {actualServiceRequest:X}")
        Dim ActualErrorAvailable As Boolean = device.StatusSubsystemBase.ErrorAvailable
        Assert.IsTrue(ActualErrorAvailable, $"Error is expected")
        Dim ActualLastError As String = device.StatusSubsystemBase.LastDeviceError.ToString
        Dim ExpectedLastError As String = "-285,TSP Syntax error at line 1: unexpected symbol near `*',level=20"
        Assert.AreEqual(ExpectedLastError, ActualLastError, True, $"Expected error for erroneous command: {erroneousCommand}")
    End Sub

#End Region

#Region " RESOURCE CONTROL "

    ''' <summary> Check selected resource name. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="control"> The control. </param>
    Public Shared Sub CheckSelectedResourceName(control As VI.Instrument.ResourceControlBase)
        If Not TestInfo.ResourceLocated Then Assert.Inconclusive($"{TestInfo.ResourceTitle} not found")
        If control Is Nothing Then Throw New ArgumentNullException(NameOf(control))
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean = True
        Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #1")

        Dim e As New isr.Core.Pith.ActionEventArgs
        Dim factory As SessionFactory = control.SessionFactory
        factory.EnumerateResources()
        actualBoolean = factory.HasResources
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Factory failed to enumerate resource names")

        actualBoolean = control.InternalResourceNamesCount > 0
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Connector failed to list resource names")

        actualBoolean = factory.TrySelectResource(TestInfo.ResourceName, e)
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed selecting {TestInfo.ResourceName} selected {factory.SelectedResourceName}")

        Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #2")

        actualBoolean = factory.SelectedResourceExists
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"Resource {TestInfo.ResourceName} not found")

        Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #3")

        Dim actualResource As String = factory.SelectedResourceName
        Dim expectedResource As String = TestInfo.ResourceName
        Assert.AreEqual(expectedResource, actualResource, $"Factory selected resource mismatch")

        actualResource = control.InternalSelectedResourceName
        expectedResource = TestInfo.ResourceName
        Assert.AreEqual(expectedResource, actualResource, $"Connector selected resource mismatch")

        TestInfo.AssertMessageQueue()

    End Sub

    ''' <summary> Opens a session. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="trialNumber"> The trial number. </param>
    ''' <param name="control">     The control. </param>
    Public Shared Sub OpenSession(ByVal trialNumber As Integer, ByVal control As VI.Instrument.ResourceControlBase)
        If Not TestInfo.ResourceLocated Then Assert.Inconclusive($"{TestInfo.ResourceTitle} not found")
        If control Is Nothing Then Throw New ArgumentNullException(NameOf(control))
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean

        Dim e As New isr.Core.Pith.ActionEventArgs
        Dim device As VI.DeviceBase = control.InternalDevice
        If Not device.IsDeviceOpen Then
            device.TryOpenSession(TestInfo.ResourceName, TestInfo.ResourceTitle, e)
            actualBoolean = e.Cancel
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} opening session {TestInfo.ResourceName} canceled; {e.Details}")
        End If

        actualBoolean = control.IsConnected
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} opening session {TestInfo.ResourceName} control not connected")

        actualBoolean = device.IsDeviceOpen
        expectedBoolean = True
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} opening session {TestInfo.ResourceName} device not open")

        actualBoolean = control.InternalIsDeviceOwner
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} failed; control must not be device owner")

        TestInfo.CheckModel(device.StatusSubsystemBase)
        TestInfo.AssertMessageQueue()

    End Sub

    ''' <summary> Closes a session. </summary>
    ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    ''' <param name="trialNumber"> The trial number. </param>
    ''' <param name="control">     The control. </param>
    Public Shared Sub CloseSession(ByVal trialNumber As Integer, ByVal control As VI.Instrument.ResourceControlBase)
        If Not TestInfo.ResourceLocated Then Assert.Inconclusive($"{TestInfo.ResourceTitle} not found")
        If control Is Nothing Then Throw New ArgumentNullException(NameOf(control))
        Dim expectedBoolean As Boolean = True
        Dim actualBoolean As Boolean

        Dim device As VI.DeviceBase = control.InternalDevice
        device.TryCloseSession()

        actualBoolean = control.IsConnected
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} closing session {TestInfo.ResourceName} control still connected")

        actualBoolean = device.IsDeviceOpen
        expectedBoolean = False
        Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} closing session {TestInfo.ResourceName} device still open")
        TestInfo.AssertMessageQueue()

    End Sub

    ''' <summary> Opens close session. </summary>
    ''' <param name="trialNumber"> The trial number. </param>
    ''' <param name="control">     The control. </param>
    Public Shared Sub OpenCloseSession(ByVal trialNumber As Integer, ByVal control As VI.Instrument.ResourceControlBase)
        Using device As Device = Device.Create
            TestInfo.OpenSession(trialNumber, control)
            TestInfo.CloseSession(trialNumber, control)
        End Using
    End Sub

#End Region

End Class

