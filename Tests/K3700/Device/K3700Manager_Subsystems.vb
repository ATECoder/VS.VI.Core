Imports isr.Core.Pith.StopwatchExtensions
Namespace K3700.Tests

    Partial Friend NotInheritable Class K3700Manager

#Region " CHECK SUSBSYSTEMS "

        ''' <summary> Check session. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="session"> The session. </param>
        Public Shared Sub CheckSession(ByVal session As VI.Pith.SessionBase)
            If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
            Dim actualCommand As String = session.IsAliveCommand
            Dim expectedCommand As String = K3700SubsystemsInfo.Get.KeepAliveCommand
            Assert.AreEqual(expectedCommand, actualCommand, $"Keep alive command.")

            actualCommand = session.IsAliveQueryCommand
            expectedCommand = K3700SubsystemsInfo.Get.KeepAliveQueryCommand
            Assert.AreEqual(expectedCommand, actualCommand, $"Keep alive query command.")

            Dim expectedErrorAvailableBits As Integer = VI.Pith.ServiceRequests.ErrorAvailable
            Dim actualErrorAvailableBits As Integer = session.ErrorAvailableBits
            Assert.AreEqual(expectedErrorAvailableBits, actualErrorAvailableBits, $"Error available bits on creating device.")
        End Sub


        ''' <summary> Check line frequency. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="subsystem"> The subsystem. </param>
        Public Shared Sub CheckLineFrequency(ByVal subsystem As VI.StatusSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Dim actualFrequency As Double = subsystem.LineFrequency.GetValueOrDefault(0)
            Assert.AreEqual(K3700SubsystemsInfo.Get.LineFrequency, actualFrequency, $"{NameOf(VI.StatusSubsystemBase.LineFrequency)} is {actualFrequency}; expected {K3700SubsystemsInfo.Get.LineFrequency}")
        End Sub

        ''' <summary> Check integration period. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="subsystem"> The subsystem. </param>
        Public Shared Sub CheckIntegrationPeriod(ByVal subsystem As VI.StatusSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Dim expectedPowerLineCycles As Double = K3700SubsystemsInfo.Get.InitialPowerLineCycles
            Dim expectedIntegrationPeriod As TimeSpan = VI.StatusSubsystemBase.FromSecondsPrecise(expectedPowerLineCycles / K3700SubsystemsInfo.Get.LineFrequency)
            Dim actualIntegrationPeriod As TimeSpan = VI.StatusSubsystemBase.FromPowerLineCycles(expectedPowerLineCycles)
            Assert.AreEqual(expectedIntegrationPeriod, actualIntegrationPeriod,
                                    $"Integration period for {expectedPowerLineCycles} power line cycles is {actualIntegrationPeriod}; expected {expectedIntegrationPeriod}")

            Dim actualPowerLineCycles As Double = VI.StatusSubsystemBase.ToPowerLineCycles(actualIntegrationPeriod)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, K3700SubsystemsInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                                    $"Power line cycles is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")
        End Sub

        ''' <summary> Check model. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        Public Shared Sub CheckModel(ByVal subsystem As VI.StatusSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Assert.AreEqual(K3700ResourceInfo.Get.ResourceModel, subsystem.VersionInfo.Model, $"Version Info Model {subsystem.ResourceNameCaption} iIdentity: '{subsystem.VersionInfo.Identity}'", Globalization.CultureInfo.CurrentCulture)
        End Sub

        ''' <summary> Opens close session. </summary>
        Public Shared Sub CheckDeviceErrors(ByVal subsystem As VI.StatusSubsystemBase)
            If Not K3700ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K3700ResourceInfo.Get.ResourceTitle} not found")
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Dim e As New Core.Pith.ActionEventArgs
            subsystem.TrySafeQueryDeviceErrors(e)
            Assert.IsFalse(e.Cancel, $"{subsystem.ResourceNameCaption}  failed reading existing errors {e.Details}")
            Assert.IsFalse(subsystem.ErrorAvailable, $"{subsystem.ResourceNameCaption} error available bit {subsystem.Session.ServiceRequestStatus:X} is on; last device error: {subsystem.LastDeviceError}")
            Assert.IsTrue(String.IsNullOrWhiteSpace(subsystem.DeviceErrorsReport), $"{subsystem.ResourceNameCaption} device errors: {subsystem.DeviceErrorsReport}")
            Assert.IsFalse(subsystem.LastDeviceError.IsError, $"{subsystem.ResourceNameCaption} last device error: {subsystem.LastDeviceError}")
        End Sub

        ''' <summary> Opens close session. </summary>
        Public Shared Sub ClearSessionCheckDeviceErrors(ByVal device As VI.DeviceBase)
            If Not K3700ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K3700ResourceInfo.Get.ResourceTitle} not found")
            If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
            device.Session.Clear()
            K3700Manager.CheckDeviceErrors(device.StatusSubsystemBase)
        End Sub

        ''' <summary> Check termination. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="session"> The session. </param>
        Public Shared Sub CheckTermination(ByVal session As VI.Pith.SessionBase)
            If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
            Dim actualReadTerminationEnabled As Boolean = session.TerminationCharacterEnabled
            Dim expectedReadTerminationEnabled As Boolean = K3700SubsystemsInfo.Get.ReadTerminationEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Initial read termination enabled")

            expectedReadTerminationEnabled = K3700SubsystemsInfo.Get.ReadTerminationEnabled
            session.TerminationCharacterEnabled = expectedReadTerminationEnabled
            actualReadTerminationEnabled = session.TerminationCharacterEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Requested read termination")

            Dim actualTermination As Integer = session.TerminationCharacter
            Dim expectedTermination As Integer = K3700SubsystemsInfo.Get.TerminationCharacter
            Assert.AreEqual(expectedTermination, actualTermination, $"Termination character value")
            Assert.AreEqual(CByte(AscW(session.Termination(0))), session.TerminationCharacter, $"First termination character value")
        End Sub

        ''' <summary> Check channel subsystem information. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="subsystem"> The subsystem. </param>
        Public Shared Sub CheckChannelSubsystemInfo(ByVal subsystem As VI.ChannelSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Assert.IsTrue(String.IsNullOrWhiteSpace(subsystem.ClosedChannels), $"Scan list {subsystem.ClosedChannels}; expected empty")
        End Sub

        ''' <summary> Check reading device errors. </summary>
        ''' <param name="device"> The device. </param>
        Public Shared Sub CheckReadingDeviceErrors(ByVal device As VI.DeviceBase)
            If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
            ' send an erroneous command
            Dim erroneousCommand As String = K3700SubsystemsInfo.Get.ErroneousCommand
            device.StatusSubsystemBase.Write(erroneousCommand)

            ' allow the device time to register the error.
            If K3700SubsystemsInfo.Get.ErrorAvailableMillisecondsDelay > 0 Then
                Stopwatch.StartNew.Wait(TimeSpan.FromMilliseconds(K3700SubsystemsInfo.Get.ErrorAvailableMillisecondsDelay))
            End If

            ' read the service request status; this should generate an error available 
            device.StatusSubsystemBase.ReadServiceRequestStatus()
            ' check the error bits
            Dim actualServiceRequest As VI.Pith.ServiceRequests = device.StatusSubsystemBase.ErrorAvailableBits
            Dim expectedSeriveRequest As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.ErrorAvailable
            Assert.AreEqual(expectedSeriveRequest, actualServiceRequest, $"Error bits expected {expectedSeriveRequest:X} <> actual {actualServiceRequest:X}")
            ' check the error available status
            actualServiceRequest = device.StatusSubsystemBase.ServiceRequestStatus
            Assert.IsTrue((actualServiceRequest And expectedSeriveRequest) = expectedSeriveRequest, $"Error bits {expectedSeriveRequest:X} are expected in {actualServiceRequest:X}")
            Dim actualErrorAvailable As Boolean = device.StatusSubsystemBase.ErrorAvailable
            Assert.IsTrue(actualErrorAvailable, $"Error Is expected")
            Dim actualLastError As String = device.StatusSubsystemBase.LastDeviceError.ToString
            Dim ExpectedLastError As String = "-285,TSP Syntax Error at line 1: unexpected symbol near `*',level=20"
            Assert.AreEqual(ExpectedLastError, actualLastError, True, $"Expected error for erroneous command: {erroneousCommand}")
        End Sub

#End Region

    End Class

End Namespace

