Imports isr.Core.Pith.StopwatchExtensions
Namespace K2450.Tests

    ''' <summary>
    ''' Static class for managing the common functions.
    ''' </summary>
    Partial Friend NotInheritable Class K2450Manager

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
            If Not K2450ResourceInfo.Get.ResourcePinged Then Assert.Inconclusive($"{K2450ResourceInfo.Get.ResourceTitle} not found")
            Dim e As New Core.Pith.ActionEventArgs
            Dim actualBoolean As Boolean = device.TryOpenSession(K2450ResourceInfo.Get.ResourceName, K2450ResourceInfo.Get.ResourceTitle, e)
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

#Region " COMMANDS "

        ''' <summary> Toggle output. </summary>
        ''' <param name="device">        The device. </param>
        ''' <param name="outputEnabled"> True to enable, false to disable the output. </param>
        Public Shared Sub ToggleOutput(ByVal device As VI.Tsp2.K2450.Device, ByVal outputEnabled As Boolean)
            If device Is Nothing Then Throw New ArgumentNullException(NameOf(device))
            Dim expectedOutputEnabled As Boolean = outputEnabled
            Dim actualOutputEnabled As Boolean = device.SourceSubsystem.ApplyOutputEnabled(expectedOutputEnabled).GetValueOrDefault(Not expectedOutputEnabled)
            Assert.AreEqual(expectedOutputEnabled, actualOutputEnabled, $"{GetType(VI.Tsp2.SourceSubsystemBase)}.{NameOf(VI.Tsp2.SourceSubsystemBase.OutputEnabled)} is {actualOutputEnabled}; expected {expectedOutputEnabled}")
        End Sub

#End Region

    End Class

End Namespace

