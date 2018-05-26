Imports isr.Core.Pith.StopwatchExtensions
Namespace K7500.Tests

    Partial Friend NotInheritable Class K7510Manager

#Region " RESOURCE CONTROL "

        ''' <summary> Check selected resource name. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="control"> The control. </param>
        Public Shared Sub CheckSelectedResourceName(control As VI.Instrument.ResourceControlBase)
            If control Is Nothing Then Throw New ArgumentNullException(NameOf(control))
            Dim expectedBoolean As Boolean = True
            Dim actualBoolean As Boolean = True
            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #1")

            Dim e As New Core.Pith.ActionEventArgs
            Dim factory As VI.SessionFactory = control.SessionFactory
            factory.EnumerateResources()
            actualBoolean = factory.HasResources
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Factory failed to enumerate resource names")

            actualBoolean = control.InternalResourceNamesCount > 0
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Connector failed to list resource names")

            actualBoolean = factory.TrySelectResource(K7510ResourceInfo.Get.ResourceName, e)
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Failed selecting {K7510ResourceInfo.Get.ResourceName} selected {factory.SelectedResourceName}")

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #2")

            actualBoolean = factory.SelectedResourceExists
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"Resource {K7510ResourceInfo.Get.ResourceName} not found")

            Assert.IsTrue(control.Talker IsNot Nothing, $"Talker is nothing #3")

            Dim actualResource As String = factory.SelectedResourceName
            Dim expectedResource As String = K7510ResourceInfo.Get.ResourceName
            Assert.AreEqual(expectedResource, actualResource, $"Factory selected resource mismatch")

            actualResource = control.InternalSelectedResourceName
            expectedResource = K7510ResourceInfo.Get.ResourceName
            Assert.AreEqual(expectedResource, actualResource, $"Connector selected resource mismatch")
        End Sub

        ''' <summary> Opens a session. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="trialNumber"> The trial number. </param>
        ''' <param name="control">     The control. </param>
        Public Shared Sub OpenSession(ByVal trialNumber As Integer, ByVal control As VI.Instrument.ResourceControlBase)
            If Not K7510ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K7510ResourceInfo.Get.ResourceTitle} not found")
            If control Is Nothing Then Throw New ArgumentNullException(NameOf(control))
            Dim expectedBoolean As Boolean = True
            Dim actualBoolean As Boolean

            Dim e As New Core.Pith.ActionEventArgs
            Dim device As VI.DeviceBase = control.InternalDevice
            If Not device.IsDeviceOpen Then
                device.TryOpenSession(K7510ResourceInfo.Get.ResourceName, K7510ResourceInfo.Get.ResourceTitle, e)
                actualBoolean = e.Cancel
                expectedBoolean = False
                Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} opening session {K7510ResourceInfo.Get.ResourceName} canceled; {e.Details}")
            End If

            actualBoolean = control.IsConnected
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} opening session {K7510ResourceInfo.Get.ResourceName} control not connected")

            actualBoolean = device.IsDeviceOpen
            expectedBoolean = True
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} opening session {K7510ResourceInfo.Get.ResourceName} device not open")

            actualBoolean = control.InternalIsDeviceOwner
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} failed; control must not be device owner")

            K7510Manager.CheckModel(device.StatusSubsystemBase)
        End Sub

        ''' <summary> Closes a session. </summary>
        ''' <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        ''' <param name="trialNumber"> The trial number. </param>
        ''' <param name="control">     The control. </param>
        Public Shared Sub CloseSession(ByVal trialNumber As Integer, ByVal control As VI.Instrument.ResourceControlBase)
            If Not K7510ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K7510ResourceInfo.Get.ResourceTitle} not found")
            If control Is Nothing Then Throw New ArgumentNullException(NameOf(control))
            Dim expectedBoolean As Boolean = True
            Dim actualBoolean As Boolean

            Dim device As VI.DeviceBase = control.InternalDevice
            device.TryCloseSession()

            actualBoolean = control.IsConnected
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} closing session {K7510ResourceInfo.Get.ResourceName} control still connected")

            actualBoolean = device.IsDeviceOpen
            expectedBoolean = False
            Assert.AreEqual(expectedBoolean, actualBoolean, $"{trialNumber} closing session {K7510ResourceInfo.Get.ResourceName} device still open")
        End Sub

        ''' <summary> Opens close session. </summary>
        ''' <param name="trialNumber"> The trial number. </param>
        ''' <param name="control">     The control. </param>
        Public Shared Sub OpenCloseSession(ByVal trialNumber As Integer, ByVal control As VI.Instrument.ResourceControlBase)
            Using device As VI.Tsp2.K7500.Device = VI.Tsp2.K7500.Device.Create
                K7510Manager.OpenSession(trialNumber, control)
                K7510Manager.CloseSession(trialNumber, control)
            End Using
        End Sub

#End Region

    End Class

End Namespace

