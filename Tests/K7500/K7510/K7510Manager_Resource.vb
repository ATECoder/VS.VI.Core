Imports isr.Core.Pith.StopwatchExtensions
Namespace K7500.Tests

    ''' <summary>
    ''' Static class for managing the common functions.
    ''' </summary>
    Partial Friend NotInheritable Class K7510Manager

#Region " CONSTRUCTORS "

        Private Sub New()
            MyBase.New
        End Sub

#End Region

#Region " DEVICE OPEN, CLOSE "

        ''' <summary> Opens a session. </summary>
        ''' <param name="device"> The device. </param>
        Public Shared Sub OpenSession(ByVal device As VI.DeviceBase)
            If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
            If Not K7510ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K7510ResourceInfo.Get.ResourceTitle} not found")
            Dim e As New Core.Pith.ActionEventArgs
            Dim actualBoolean As Boolean = device.TryOpenSession(K7510ResourceInfo.Get.ResourceName, K7510ResourceInfo.Get.ResourceTitle, e)
            Assert.IsTrue(actualBoolean, $"Failed to open session: {e.Details}")
        End Sub

        ''' <summary> Closes a session. </summary>
        ''' <param name="device"> The device. </param>
        Public Shared Sub CloseSession(ByVal device As VI.DeviceBase)
            If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
            device.Session.Clear()
            device.CloseSession()
            Assert.IsFalse(device.IsDeviceOpen, $"Failed closing session to {device.ResourceName}")
        End Sub

#End Region

    End Class

End Namespace

