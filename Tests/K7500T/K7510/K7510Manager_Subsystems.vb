Imports isr.Core.Pith.StopwatchExtensions
Namespace K7500.Tests

    Partial Friend NotInheritable Class K7510Manager

#Region " CHECK SUSBSYSTEMS "

        ''' <summary> Check session. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="session"> The session. </param>
        Public Shared Sub CheckSession(ByVal session As VI.Pith.SessionBase)
            If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
            If Not K7510ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K7510ResourceInfo.Get.ResourceTitle} not found")
            Dim actualCommand As String = session.IsAliveCommand
            Dim expectedCommand As String = K7510SubsystemsInfo.Get.KeepAliveCommand
            Assert.AreEqual(expectedCommand, actualCommand, $"Keep alive command.")

            actualCommand = session.IsAliveQueryCommand
            expectedCommand = K7510SubsystemsInfo.Get.KeepAliveQueryCommand
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
            Assert.AreEqual(K7510SubsystemsInfo.Get.LineFrequency, actualFrequency, $"{NameOf(VI.StatusSubsystemBase.LineFrequency)} is {actualFrequency}; expected {K7510SubsystemsInfo.Get.LineFrequency}")
        End Sub

        ''' <summary> Check integration period. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="subsystem"> The subsystem. </param>
        Public Shared Sub CheckIntegrationPeriod(ByVal subsystem As VI.StatusSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Dim expectedPowerLineCycles As Double = K7510SubsystemsInfo.Get.InitialPowerLineCycles
            Dim expectedIntegrationPeriod As TimeSpan = VI.StatusSubsystemBase.FromSecondsPrecise(expectedPowerLineCycles / K7510SubsystemsInfo.Get.LineFrequency)
            Dim actualIntegrationPeriod As TimeSpan = VI.StatusSubsystemBase.FromPowerLineCycles(expectedPowerLineCycles)
            Assert.AreEqual(expectedIntegrationPeriod, actualIntegrationPeriod,
                                    $"Integration period for {expectedPowerLineCycles} power line cycles is {actualIntegrationPeriod}; expected {expectedIntegrationPeriod}")

            Dim actualPowerLineCycles As Double = VI.StatusSubsystemBase.ToPowerLineCycles(actualIntegrationPeriod)
            Assert.AreEqual(expectedPowerLineCycles, actualPowerLineCycles, K7510SubsystemsInfo.Get.LineFrequency / TimeSpan.TicksPerSecond,
                                    $"Power line cycles is {actualPowerLineCycles:G5}; expected {expectedPowerLineCycles:G5}")
        End Sub

        ''' <summary> Check model. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        Public Shared Sub CheckModel(ByVal subsystem As VI.StatusSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Assert.AreEqual(K7510ResourceInfo.Get.ResourceModel, subsystem.VersionInfo.Model, $"Version Info Model {subsystem.ResourceNameCaption} Identity: '{subsystem.VersionInfo.Identity}'", Globalization.CultureInfo.CurrentCulture)
        End Sub

        ''' <summary> Opens close session. </summary>
        Public Shared Sub CheckDeviceErrors(ByVal subsystem As VI.StatusSubsystemBase)
            If Not K7510ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K7510ResourceInfo.Get.ResourceTitle} not found")
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Dim e As New Core.Pith.ActionEventArgs
            subsystem.TrySafeQueryDeviceErrors(e)
            Assert.IsFalse(e.Cancel, $"{subsystem.ResourceNameCaption}  failed reading existing errors {e.Details}")
            Assert.IsFalse(subsystem.ErrorAvailable, $"{subsystem.ResourceNameCaption} error available bit {subsystem.Session.ServiceRequestStatus:X} is on; last device error: {subsystem.LastDeviceError}")
            Assert.IsTrue(String.IsNullOrWhiteSpace(subsystem.DeviceErrorsReport), $"{subsystem.ResourceNameCaption} device errors: {subsystem.DeviceErrorsReport}")
            Assert.IsFalse(subsystem.LastDeviceError.IsError, $"{subsystem.ResourceNameCaption} last device error: {subsystem.LastDeviceError}")

            Dim deviceError As New VI.DeviceError()
            deviceError.Parse(K7510SubsystemsInfo.Get.ExpectedCompoundErrorMessage)
            Assert.AreEqual(K7510SubsystemsInfo.Get.ExpectedErrorMessage, deviceError.ErrorMessage, "Expected error message")
            Assert.AreEqual(K7510SubsystemsInfo.Get.ExpectedErrorNumber, deviceError.ErrorNumber, "Expected error number")
            Assert.AreEqual(K7510SubsystemsInfo.Get.ExpectedErrorLevel, deviceError.ErrorLevel, "Expected error level")

        End Sub

        ''' <summary> Opens close session. </summary>
        Public Shared Sub ClearSessionCheckDeviceErrors(ByVal device As VI.DeviceBase)
            If Not K7510ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K7510ResourceInfo.Get.ResourceTitle} not found")
            If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
            device.Session.Clear()
            K7510Manager.CheckDeviceErrors(device.StatusSubsystemBase)
        End Sub

        ''' <summary> Check termination. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="session"> The session. </param>
        Public Shared Sub CheckTermination(ByVal session As VI.Pith.SessionBase)
            If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))
            Dim actualReadTerminationEnabled As Boolean = session.TerminationCharacterEnabled
            Dim expectedReadTerminationEnabled As Boolean = K7510SubsystemsInfo.Get.ReadTerminationEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Initial read termination enabled")

            expectedReadTerminationEnabled = K7510SubsystemsInfo.Get.ReadTerminationEnabled
            session.TerminationCharacterEnabled = expectedReadTerminationEnabled
            actualReadTerminationEnabled = session.TerminationCharacterEnabled
            Assert.AreEqual(expectedReadTerminationEnabled, actualReadTerminationEnabled, $"Requested read termination")

            Dim actualTermination As Integer = session.TerminationCharacter
            Dim expectedTermination As Integer = K7510SubsystemsInfo.Get.TerminationCharacter
            Assert.AreEqual(expectedTermination, actualTermination, $"Termination character value")
            Assert.AreEqual(CByte(AscW(session.Termination(0))), session.TerminationCharacter, $"First termination character value")
        End Sub

        Public Shared Sub CheckMultimeterSubsystemInfo(ByVal subsystem As VI.MultimeterSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
        End Sub

        Public Shared Sub CheckBufferSubsystemInfo(ByVal subsystem As VI.BufferSubsystemBase)
            If subsystem Is Nothing Then Throw New ArgumentNullException(NameOf(subsystem))
            Dim actualCapacilty As Integer = subsystem.QueryCapacity.GetValueOrDefault(-1)
            Dim expectedcapacilty As Integer = K7510SubsystemsInfo.Get.BufferCapacity
            Assert.AreEqual(expectedcapacilty, actualCapacilty, $"Buffer capacity")
            Dim actualFirstPointNumber As Integer = subsystem.QueryFirstPointNumber.GetValueOrDefault(-1)
            Dim expectedFirstPointNumber As Integer = K7510SubsystemsInfo.Get.BufferFirstPointNumber
            Assert.AreEqual(expectedcapacilty, actualCapacilty, $"Buffer First Point Number")
            Dim actualLastPointNumber As Integer = subsystem.QueryLastPointNumber.GetValueOrDefault(-1)
            Dim expectedLastPointNumber As Integer = K7510SubsystemsInfo.Get.BufferLastPointNumber
            Assert.AreEqual(expectedcapacilty, actualCapacilty, $"Buffer Last Point Number")
            Dim expectedFillOnceEnabled As Boolean = K7510SubsystemsInfo.Get.BufferFillOnceEnabled
            Dim actualFillOnceEnabled As Boolean = subsystem.FillOnceEnabled.GetValueOrDefault(Not expectedFillOnceEnabled)
            Assert.AreEqual(expectedFillOnceEnabled, actualFillOnceEnabled, $"Initial fill once enabled")
        End Sub

        ''' <summary> Check reading device errors. </summary>
        ''' <param name="device"> The device. </param>
        Public Shared Sub CheckReadingDeviceErrors(ByVal device As VI.DeviceBase)
            If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
            ' send an erroneous command
            Dim erroneousCommand As String = K7510SubsystemsInfo.Get.ErroneousCommand
            device.StatusSubsystemBase.Write(erroneousCommand)

            Dim appliedDelay As Integer = 0
            ' a wait is necessary for the device to register the errors.
            If K7510SubsystemsInfo.Get.ErrorAvailableMillisecondsDelay > 0 Then
                appliedDelay = K7510SubsystemsInfo.Get.ErrorAvailableMillisecondsDelay
                Dim sw As Stopwatch = Stopwatch.StartNew
                sw.Wait(TimeSpan.FromMilliseconds(K7510SubsystemsInfo.Get.ErrorAvailableMillisecondsDelay))
                appliedDelay = CInt(sw.Elapsed.TotalMilliseconds)
            End If

            ' read the service request status; this should generate an error available 
            Dim value As Integer = device.StatusSubsystemBase.ReadServiceRequestStatus()

            ' check the error bits
            Dim actualServiceRequest As VI.Pith.ServiceRequests = device.StatusSubsystemBase.ErrorAvailableBits
            Dim expectedSeriveRequest As VI.Pith.ServiceRequests = VI.Pith.ServiceRequests.ErrorAvailable
            Assert.AreEqual(expectedSeriveRequest, actualServiceRequest, $"Error bits expected {expectedSeriveRequest:X} <> actual {actualServiceRequest:X}")

            ' check the error available status
            Assert.IsTrue((value And expectedSeriveRequest) = expectedSeriveRequest, $"Error bits {expectedSeriveRequest:X} are expected in value: {value:X}; applied {appliedDelay} ms delay")

            ' check the error available status
            actualServiceRequest = device.StatusSubsystemBase.ServiceRequestStatus
            Assert.IsTrue((actualServiceRequest And expectedSeriveRequest) = expectedSeriveRequest, $"Error bits {expectedSeriveRequest:X} are expected in {NameOf(VI.StatusSubsystemBase.ServiceRequestStatus)}: {actualServiceRequest:X}")

            Dim actualErrorAvailable As Boolean = device.StatusSubsystemBase.ErrorAvailable
            Assert.IsTrue(actualErrorAvailable, $"Error is expected")

            Assert.AreEqual(K7510SubsystemsInfo.Get.ExpectedErrorMessage, device.StatusSubsystemBase.LastDeviceError.ErrorMessage, "Expected error message")
            Assert.AreEqual(K7510SubsystemsInfo.Get.ExpectedErrorNumber, device.StatusSubsystemBase.LastDeviceError.ErrorNumber, "Expected error number")
            Assert.AreEqual(K7510SubsystemsInfo.Get.ExpectedErrorLevel, device.StatusSubsystemBase.LastDeviceError.ErrorLevel, "Expected error level")
        End Sub

#End Region

    End Class

End Namespace

